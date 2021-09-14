using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<Cruise>> FindCruises(int RouteId, DateTime Date) //finner cruiser på bestemt rute og uke dag (man antar at disse cruiser skjer hver uke derfor dato er ikke viktig her)
        {

            return await _DB.Cruises.Where(c => c.Route.Id == RouteId && c.Departure_DayOfWeek == ((int)Date.DayOfWeek)).ToListAsync();
        }

        public async Task<List<Cruise>> CheckAvailability(List<Cruise> Cruises, int PassengersAmount, DateTime DepartureDate ) //sjekker tilgjengelighet for liste med utvalgte cruiser og forkaster disse som er fulle
        {

            List<Cruise> AvailableCruises = new List<Cruise>();

            foreach (var Cruise in Cruises) //loop through 
            {
                if (await CheckAvailability(Cruise, PassengersAmount, DepartureDate))
                {
                    AvailableCruises.Add(Cruise);
                }
            }

            return AvailableCruises;
        }

        private async Task<bool> CheckAvailability(Cruise Cruise, int PassengersAmount, DateTime CruiseDate) //hjelpe metode som sjekker tilgjengelighet for bestemt cruise på bestemt dato
        {
            var AvailableSeats = Cruise.Max_Passengers;

            //first return list of orders on this specific cruise and this specific date. The amount of booked seats are calculated by suming total registered passengers and underage passengers
            var BookedSeats = await _DB.Orders.Where(o => o.Cruise == Cruise && o.Cruise_Date == CruiseDate).SumAsync(o => o.Passengers + o.Passenger_Underage);

            return BookedSeats + PassengersAmount <= AvailableSeats;
            
        }

        public async Task<Cruise> FindCruise(int CruiseId) //returnerer funnet cruise objekt etter cruise id
        {
            return await _DB.Cruises.FindAsync(CruiseId);
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

        public async Task RegisterOrder(OrderInformation orderInformation) //Registrerer order
        {

            if (orderInformation == null) //sjekker om order information objektet eksisterer
            {
                throw new ArgumentNullException("Object with order information is not found.");
            }
            
            Cruise cruise = await FindCruise(orderInformation.Cruise_Id); //søk etter gitt cruise

            if (cruise == null) //dersom cruise ble ikke funnet, kast exception
            {
                throw new ArgumentException("Illegal cruise id.");
            }


            if (!await CheckAvailability(cruise, orderInformation.Passengers + orderInformation.Passenger_Underage, orderInformation.Cruise_Date)) //sjekker tilgjenglighet igjen dersom antall fri plass kunne bli endret underveis
            {
                throw new ArgumentOutOfRangeException("Requested amount of seats are not available.");
            }

            if (cruise.Departure_DayOfWeek != ((int)orderInformation.Cruise_Date.DayOfWeek)) //sjekker integritet mellom valgt cruise dato og dagene gitt cruise går.
            {
                throw new ArgumentException("The provided cruise date is invalid for choosen cruise.");
            }

            Post post = await FindPost(orderInformation.Zip_Code); //søk etter gitt postnummer 

            if (post == null) //dersom gitt postnummer ble ikke funnet, lag et nytt objekt 
            {
                post.Zip_Code = orderInformation.Zip_Code;
                post.City = orderInformation.City;
            }

            Customer customer = new Customer 
            {
                Name = orderInformation.Name,
                Surname = orderInformation.Surname,
                Age = orderInformation.Age,
                Address = orderInformation.Address,
                Post = post,
                Phone = orderInformation.Phone,
                Email = orderInformation.Email
            };

            //todo? check if this customer allready exist, search after this specific object, if it is found, then use it.

            Order order = new Order
            {
                Order_Date = orderInformation.Order_Date,
                Customer = customer,
                Cruise = cruise,
                Cruise_Date = orderInformation.Cruise_Date,
                Passengers = orderInformation.Passengers,
                Passenger_Underage = orderInformation.Passenger_Underage,
                Pets = orderInformation.Pets,
                Vehicles = orderInformation.Vehicles
            };

            //todo? check if this order allready exist, search after this specific object, if it is found, then use it.
            
            _DB.Orders.Add(order);
            await _DB.SaveChangesAsync();
        }

    }
}
