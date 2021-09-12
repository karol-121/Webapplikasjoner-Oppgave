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

        public async Task<List<Route>> GetRoutes()
        {
            return await _DB.Routes.ToListAsync();
        }

        public async Task<List<Cruise>> FindCruises(int RouteId, int Departure_DayOfWeek)
        {
            return await _DB.Cruises.Where(c => c.Route.Id == RouteId && c.Departure_DayOfWeek == Departure_DayOfWeek).ToListAsync();
        }

        //check avaiblility - best at den teller opp antall ordrer med gitt cruise id og dato

        public async Task<List<Cruise>> CheckAvailability(Cruise[] Cruises, int PassengersAmount, DateTime DepartureDate )
        {
            //loop through cruises
            //for each cruise check availability
            //exclude those who dont pass the availability check
            //return modified list.

            return new List<Cruise>();
        }

        private async Task<bool> CheckAvailability(Cruise Cruise, int PassengersAmount, DateTime CruiseDate)
        {
            var AvailableSeats = Cruise.Max_Passengers;

            //this should first return list of orders on this specific cruise and this specific date. The amount of booked seats are calculated by suming total registered passengers and underage passengers
            var BookedSeats = await _DB.Orders.Where(o => o.Cruise == Cruise && o.Cruise_Date == CruiseDate).SumAsync(o => o.Passengers + o.Passenger_Underage);

            Console.WriteLine("Amount of booked seats: " + BookedSeats);


            return BookedSeats + PassengersAmount <= AvailableSeats;
            
        }

    }
}
