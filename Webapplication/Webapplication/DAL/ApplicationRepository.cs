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

        public async Task<List<Route>> GetRoutes() //henter alle ruter som fantes inn i databasen/systemet
        {
            return await _DB.Routes.OrderBy(r => r.Origin).ToListAsync();
        }

        public async Task<List<Departure>> GetDepartures(int Route_Id, DateTime Date_from, DateTime Date_to)
        {
            if (Date_from < DateTime.Today || Date_to < DateTime.Today)// dersom det spørs om dato som har vært
            {
                throw new ArgumentOutOfRangeException("date/dates can not be earlier than presents");
            }

            if (Date_to < Date_from) // dersom det spørs om intervallet som er negativ, noe som gir ikke mening
            {
                throw new ArgumentException("the interval is negative, which is not allowed");
            }

            //den brude nå returnere listen etter dato men dette kan ikke virkelig sjekkes med data som er i databasen dersom de er allerede etter order.
            return await _DB.Departures.Where(d => d.Cruise.Route.Id == Route_Id && d.Date >= Date_from && d.Date <= Date_to).OrderBy(d => d.Date).ToListAsync();
        }

        public async Task<List<Departure>> CheckAvailability(List<Departure> Departures, int Passengers) //sjekker tilgjengelighet for liste med utvalgte cruiser og forkaster disse som er fulle
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

        private async Task<bool> CheckAvailability(Departure Departure, int Passengers) //hjelpe metode som sjekker tilgjengelighet for bestemt cruise på bestemt dato
        {

            var AvailableSeats = Departure.Cruise.Max_Passengers;

            //henter alle booked plasser ved å summere antall registrerte pasasjerer fra ordrer på spesifik cruise 
            var BookedSeats = await _DB.Orders.Where(o => o.Departure == Departure).SumAsync(o => o.Passengers + o.Passengers_Underage);

            return BookedSeats + Passengers <= AvailableSeats;
            
        }

        public async Task<Departure> FindDeparture(int Departure_Id) //returnerer funnet schedule objekt etter schedule id
        {
            return await _DB.Departures.FindAsync(Departure_Id);
        }


        //summary: finner kunde etter alle attriubtter bortsatt av id (noe som er ikke viktig her), hvis det er flere like, så returneres det den første.
        public async Task<Customer> FindCustomer(Customer customer) 
        {
            return await _DB.Customers.Where(c => c.Name == customer.Name && c.Surname == customer.Surname && c.Age == customer.Age && c.Address == customer.Address && c.Post == customer.Post && c.Phone == customer.Phone && c.Email == customer.Email).FirstOrDefaultAsync();
        }

        public async Task<Post> FindPost(string Zip_Code) //finner post objekt etter postnummer
        {
            return await _DB.Posts.FindAsync(Zip_Code);
        }

        public async Task RegisterOrder(OrderInformation OrderInformation) //Registrerer order
        {

            Departure departure = await FindDeparture(OrderInformation.Departure_Id);

            if (departure == null) //sjekkes om utreise som det kjøpes bilett for faktisk eksisterer.
            {
                throw new ArgumentException("Invalid departure id: Departure not found.");
            }

            if (departure.Date.CompareTo(DateTime.Now) < 0) // sjekkes om det kjøpes ikke bilett for et reise som har skjedd.
            {
                throw new ArgumentOutOfRangeException("Invalid departure: This departure is older than present");
            }

            int Passengers = OrderInformation.Passengers + OrderInformation.Passengers_Underage;

            if (Passengers < 1) //sjekkes for negative antall personer, dette må gjøres her igjen dersom herfra akseseres det kun privat hjelpe metode "checkAvailability"
            {
                throw new ArgumentOutOfRangeException("Amount of passengers can not be lower than 1");
            }

            if (!await CheckAvailability(departure, Passengers)) //sjekker tilgjenglighet igjen dersom antall fri plass kunne bli endret underveis
            {
                throw new ArgumentOutOfRangeException("Requested amount of seats are not available.");
            }

            Post post = await FindPost(OrderInformation.Zip_Code); //søk etter gitt postnummer 

            if (post == null) //dersom gitt postnummer ble ikke funnet, lag et nytt objekt 
            {
                post = new Post
                {
                    Zip_Code = OrderInformation.Zip_Code,
                    City = OrderInformation.City.ToUpper()
                };
                
            }

            Customer customer = new Customer 
            {
                Name = OrderInformation.Name.ToUpper(),
                Surname = OrderInformation.Surname.ToUpper(),
                Age = OrderInformation.Age,
                Address = OrderInformation.Address.ToUpper(),
                Post = post,
                Phone = OrderInformation.Phone.ToUpper(),
                Email = OrderInformation.Email.ToUpper()
            };

            var customer_duplicate = await FindCustomer(customer);

            if (customer_duplicate != null) //hvis det finnes en lik customer fra før, bruk den istedenfor å lagre et nytt.
            {
                customer = customer_duplicate;
            }

            Order order = new Order
            {
                Order_Date = DateTime.Now,
                Customer = customer,
                Departure = departure,
                Passengers = OrderInformation.Passengers,
                Passengers_Underage = OrderInformation.Passengers_Underage,
                Pets = OrderInformation.Pets,
                Vehicles = OrderInformation.Vehicles
            };

            _DB.Orders.Add(order);
            await _DB.SaveChangesAsync();
        }

    }
}
