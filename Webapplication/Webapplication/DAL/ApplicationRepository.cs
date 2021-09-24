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
            //todo: order etter departures alfabetisk
            return await _DB.Routes.ToListAsync();
        }

        public async Task<List<Departure>> GetDepartures(int Route_Id, DateTime Date_from, DateTime Date_to)
        {
            //todo: add check that will assume that dates are not earlier than presents

            //todo: order etter dato 
            return await _DB.Departures.Where(d => d.Cruise.Route.Id == Route_Id && d.Date >= Date_from && d.Date <= Date_to).ToListAsync();
        }

        public async Task<List<Departure>> CheckAvailability(List<Departure> Departures, int Passengers) //sjekker tilgjengelighet for liste med utvalgte cruiser og forkaster disse som er fulle
        {
            if (Passengers < 1)
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

        public async Task<Post> FindPost(string Zip_Code) //finner post objekt etter postnummer
        {
            return await _DB.Posts.FindAsync(Zip_Code);
        }

        public async Task RegisterOrder(OrderInformation OrderInformation) //Registrerer order
        {

            Departure departure = await FindDeparture(OrderInformation.Departure_Id);

            if (departure == null)
            {
                throw new ArgumentException("Invalid departure id: departure not found.");
            }

            if (departure.Date.CompareTo(DateTime.Now) < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid departure: This departure is older than present");
            }

            int Passengers = OrderInformation.Passengers + OrderInformation.Passengers_Underage;

            if (Passengers < 1)
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
                    City = OrderInformation.City
                };
                
            }

            Customer customer = new Customer 
            {
                Name = OrderInformation.Name,
                Surname = OrderInformation.Surname,
                Age = OrderInformation.Age,
                Address = OrderInformation.Address,
                Post = post,
                Phone = OrderInformation.Phone,
                Email = OrderInformation.Email
            };

            //todo: check if this customer allready exist, search after this specific object, if it is found, then use it.

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
