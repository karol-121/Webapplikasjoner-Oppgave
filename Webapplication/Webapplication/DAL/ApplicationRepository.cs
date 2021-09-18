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
            return await _DB.Routes.ToListAsync();
        }

        /*public async Task<List<Cruise>> FindCruises(int RouteId, DateTime Date) //finner cruiser på bestemt rute og uke dag (man antar at disse cruiser skjer hver uke derfor dato er ikke viktig her)
        {

            return await _DB.Cruises.Where(c => c.Route.Id == RouteId && c.Departure_DayOfWeek == ((int)Date.DayOfWeek)).ToListAsync();
        }

        public async Task<List<Cruise>> CheckAvailability(List<Cruise> Cruises, int PassengersAmount, DateTime DepartureDate ) //sjekker tilgjengelighet for liste med utvalgte cruiser og forkaster disse som er fulle
        {
            //add negative check as someone could bypass the check by adding negative amount of passsengers

            List<Cruise> AvailableCruises = new List<Cruise>();

            foreach (var Cruise in Cruises) //loop through 
            {
                if (await CheckAvailability(Cruise, PassengersAmount, DepartureDate))
                {
                    AvailableCruises.Add(Cruise);
                }
            }

            return AvailableCruises;
        }*/

        private async Task<bool> CheckAvailability(Schedule Schedule, int PassengersAmount) //hjelpe metode som sjekker tilgjengelighet for bestemt cruise på bestemt dato
        {
            if (PassengersAmount < 0)// negativ antall pasasjerer er tull og defor returner false med en gang
            {
                return false;
            }
            var AvailableSeats = Schedule.Cruise.Max_Passengers;

            //henter alle booked plasser ved å summere antall registrerte pasasjerer fra ordrer på spesifik cruise 
            var BookedSeats = await _DB.Orders.Where(o => o.Schedule == Schedule).SumAsync(o => o.Passengers + o.Passengers_Underage);

            return BookedSeats + PassengersAmount <= AvailableSeats;
            
        }

        public async Task<Cruise> FindCruise(int CruiseId) //returnerer funnet cruise objekt etter cruise id
        {
            return await _DB.Cruises.FindAsync(CruiseId);
        }

        public async Task<Schedule> FindSchedule(int ScheduleId) //returnerer funnet schedule objekt etter schedule id
        {
            return await _DB.Schedules.FindAsync(ScheduleId);
        }

        public async Task<Post> FindPost(string Zip_Code) //finner post objekt etter postnummer
        {
            return await _DB.Posts.FindAsync(Zip_Code);
        }

        public async Task RegisterPost(Post post) //Registrerer post objekt
        {
            _DB.Posts.Add(post);
            await _DB.SaveChangesAsync();
        }

        public async Task<Customer> FindCustomer(Customer customer) //vet ikke om dette er nødvendig
        {
            return await _DB.Customers.FindAsync(customer);
        }

        public async Task RegisterCustomer(Customer customer) //Registrerer kunde
        {
            _DB.Customers.Add(customer);
            await _DB.SaveChangesAsync();
        }

        public async Task RegisterOrder(OrderInformation OrderInformation) //Registrerer order
        {

            if (OrderInformation == null) //sjekker om order information objektet eksisterer
            {
                throw new ArgumentNullException("Object with order information is not found.");
            }

            Schedule schedule = await FindSchedule(OrderInformation.Schedule_Id);

            if (schedule == null)
            {
                throw new ArgumentException("Invalid schedule id: schedule not found.");
            }

            if (!await CheckAvailability(schedule, OrderInformation.Passengers + OrderInformation.Passengers_Underage)) //sjekker tilgjenglighet igjen dersom antall fri plass kunne bli endret underveis
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
                Schedule = schedule,
                Passengers = OrderInformation.Passengers,
                Passengers_Underage = OrderInformation.Passengers_Underage,
                Pets = OrderInformation.Pets,
                Vehicles = OrderInformation.Vehicles
            };

            //todo? check if this order allready exist, search after this specific object, if it is found, then use it.

            _DB.Orders.Add(order);
            await _DB.SaveChangesAsync();
        }

    }
}
