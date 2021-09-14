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

        public async Task<List<Cruise>> FindCruises(int RouteId, int Departure_DayOfWeek) //finner cruiser på bestemt rute og uke dag (man antar at disse cruiser skjer hver uke derfor dato er ikke viktig her)
        {
            return await _DB.Cruises.Where(c => c.Route.Id == RouteId && c.Departure_DayOfWeek == Departure_DayOfWeek).ToListAsync();
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

            Console.WriteLine("Amount of booked seats: " + BookedSeats); //debug print, delete this afterwards


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
            //check if order inforamtion is null?
            
            Cruise cruise = await FindCruise(orderInformation.Cruise_Id); //søk etter gitt cruise

            if (cruise == null) //dersom cruise ble ikke funnet, kast exception
            {
                Console.WriteLine("damn boi this cruise id is wrong");
                //throw new ArgumentException("Illegal cruise id");
            }

            //check the availability as the booking could change when the customer was filling its order. (it would be better to reserve seats for this customer but whatever)
            //also check the integrity with cruise date and cruise's day of the week.

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

            //here i should check if this created customer exist allready so i does not duplicate, however maybe this is allready feture in the orm or whatever

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
            
            _DB.Orders.Add(order);
            await _DB.SaveChangesAsync();
        }

    }
}
