using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _DB;

        public ApplicationRepository(ApplicationDbContext DB)
        {
            _DB = DB;
        }

        //summary: henter alle ruter sortert alfabetisk etter utreise by.
        //returns: sortert liste med rute objekter.
        public async Task<List<Route>> GetRoutes() 
        {
            return await _DB.Routes.OrderBy(r => r.Origin).ToListAsync();
        }

        //summary: henter alle utreiser for bestemt route i gitt intervall inklusivt sortert etter utreise dato.
        //parameters: int Route_Id - id for route, DateTime Date_from - fra og med dato, DateTime Date_to - til og med dato.
        //returns: sortert liste med departure objekter som oppfyller krav.
        public async Task<List<Departure>> GetDepartures(int Route_Id, DateTime Date_from, DateTime Date_to)
        {
            if (Date_from < DateTime.Today || Date_to < DateTime.Today) // dersom det spørs om dato som har vært
            {
                throw new ArgumentOutOfRangeException("date/dates can not be earlier than presents");
            }

            if (Date_to < Date_from) // dersom det spørs om intervallet som er negativ, noe som gir ikke mening
            {
                throw new ArgumentException("the interval is negative, which is not allowed");
            }

            return await _DB.Departures.Where(d => d.Cruise.Route.Id == Route_Id && d.Date >= Date_from && d.Date < Date_to.AddDays(1)).OrderBy(d => d.Date).ToListAsync();
        }

        //summary: sjekker tilgjenglihet for hver departure i listen og returnerer kun de som har plass for gitt antall personer.
        //parameters: List<Departure> Departures - liste med departure objekter som skal sjekkes, int Passengers - antall personer som det skal sjekkes plass for.
        //returns: liste med departure objekter som oppfyller krav.
        public async Task<List<Departure>> CheckAvailability(List<Departure> Departures, int Passengers) 
        {
            if (Passengers < 1) //0 eller mindre personer er ikke tilgjengelig dersom dette kunne føre til bypassing sjekk (negativ antall ville subtrahere antall plasser som er booked)
            {
                throw new ArgumentOutOfRangeException("Amount of passengers can not be lower than 1");
            }

            List<Departure> AvailableDepartures = new List<Departure>();

            foreach (var Departure in Departures)
            {
                if (await CheckAvailability(Departure, Passengers))
                {
                    AvailableDepartures.Add(Departure);
                }
            }

            return AvailableDepartures;
        }

        //summary: hjelpe metode for CheckAvailability som sjekker tilgjenglighet for et departure objekt.
        //parameters: Departure Departure - departure objekt som skal sjekkes, int Passengers - antall personer som det skal sjekkes plass for.
        //returns: bool; true - hvis det er plass, false - hvis det er ikke plass.
        private async Task<bool> CheckAvailability(Departure Departure, int Passengers)
        {

            var AvailableSeats = Departure.Cruise.CruiseDetails.Max_Passengers;

            //henter alle booked plasser ved å summere antall registrerte pasasjerer fra biletter til spesifik cruise 
            var BookedSeats = await _DB.Tickets.Where(o => o.Departure == Departure).SumAsync(o => o.Passengers + o.Passengers_Underage);

            return BookedSeats + Passengers <= AvailableSeats;
            
        }

        //summary: finner departure objekt etter dens id.
        //parameters: int Departure_Id - id for departure objekt som skal finnes.
        //returns: departure objekt, null - hvis objektet ble ikke funnet.
        public async Task<Departure> FindDeparture(int Departure_Id)
        {
            return await _DB.Departures.FindAsync(Departure_Id);
        }


        //summary: finner kunde etter dens attriubtter ekskludert "id". Dersom det er flere samsvarende objekter, returners den første.
        //parameters: Customer customer - kunde objekt som skal finnes
        //returns: Customer objekt, null - hvis objektet ble ikke funnet.
        public async Task<Customer> FindCustomer(Customer customer) 
        {
            return await _DB.Customers.Where(c => c.Name == customer.Name && c.Surname == customer.Surname && c.Age == customer.Age && c.Address == customer.Address && c.Post == customer.Post && c.Phone == customer.Phone && c.Email == customer.Email).FirstOrDefaultAsync();
        }

        //summary: finner post objekt etter postnummer
        //parameters: string Zip_Code - postnummer som er postens primary key
        //returns: Post objekt, null - hvis objektet ble ikke funnet.
        public async Task<Post> FindPost(string Zip_Code)
        {
            return await _DB.Posts.FindAsync(Zip_Code);
        }

        //summary: registrerer bilett med informasjon fra orderItem objektet
        //parameters: OrderItem orderItem - objektet som inneholder informasjon nødvendig for registrering,
        //string session - unik session key som assosiaserer denne bilett med et session.
        public async Task RegisterOrderItem(OrderItem orderItem, string session)
        {

            Departure departure = await FindDeparture(orderItem.Departure_Id);

            if (departure == null) //sjekkes om utreise som det kjøpes bilett for faktisk eksisterer.
            {
                throw new ArgumentException("Invalid departure id: Departure not found.");
            }

            if (departure.Date.CompareTo(DateTime.Now) < 0) // sjekkes om det kjøpes ikke bilett for et reise som har skjedd.
            {
                throw new ArgumentOutOfRangeException("Invalid departure: This departure is older than present");
            }

            int Passengers = orderItem.Passengers + orderItem.Passengers_Underage;

            if (Passengers < 1) //sjekkes for negative antall personer, dette må gjøres her igjen dersom herfra akseseres det kun privat hjelpe metode "checkAvailability"
            {
                throw new ArgumentOutOfRangeException("Amount of passengers can not be lower than 1");
            }

            if (!await CheckAvailability(departure, Passengers)) //sjekker tilgjenglighet igjen dersom antall fri plass kunne bli endret underveis
            {
                throw new ArgumentOutOfRangeException("Requested amount of seats are not available.");
            }

            Post post = await FindPost(orderItem.Zip_Code); //søk etter gitt postnummer 

            if (post == null) //dersom gitt postnummer ble ikke funnet, lag et nytt objekt 
            {
                post = new Post
                {
                    Zip_Code = orderItem.Zip_Code,
                    City = orderItem.City.ToUpper()
                };
                
            }

            Customer customer = new Customer 
            {
                Name = orderItem.Name.ToUpper(),
                Surname = orderItem.Surname.ToUpper(),
                Age = orderItem.Age,
                Address = orderItem.Address.ToUpper(),
                Post = post,
                Phone = orderItem.Phone.ToUpper(),
                Email = orderItem.Email.ToUpper()
            };

            var customer_duplicate = await FindCustomer(customer);

            if (customer_duplicate != null) //hvis det finnes en lik customer fra før, bruk den istedenfor å lagre et nytt.
            {
                customer = customer_duplicate;
            }

            Ticket ticket = new Ticket
            {
                Session = session,
                Customer = customer,
                Departure = departure,
                Passengers = orderItem.Passengers,
                Passengers_Underage = orderItem.Passengers_Underage,
                Pets = orderItem.Pets,
                Vehicles = orderItem.Vehicles
            };

            _DB.Tickets.Add(ticket);
            await _DB.SaveChangesAsync();
        }

        //summary: fjerner alle biletter som er relatert til et session.
        //parameters: string session - streng med session verdi
        public async Task RemoveSessionTickets(string session)
        {

            var tickets = await _DB.Tickets.Where(o => o.Session.Equals(session)).ToListAsync(); //finn alle ordre som er relatert til et sesjon

            foreach (var ticket in tickets)
            {
                _DB.Tickets.Remove(ticket); //fjern hver relevant order
            }
            
            await _DB.SaveChangesAsync(); //lagre 
        }

    }
}
