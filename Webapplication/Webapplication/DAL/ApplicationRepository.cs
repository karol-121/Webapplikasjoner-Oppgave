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

            //this should first return list of orders on this specific cruise and this specific date. The amount of booked seats are calculated by suming total registered passengers and underage passengers
            var BookedSeats = await _DB.Orders.Where(o => o.Cruise == Cruise && o.Cruise_Date == CruiseDate).SumAsync(o => o.Passengers + o.Passenger_Underage);

            Console.WriteLine("Amount of booked seats: " + BookedSeats); //debug print, delete this afterwards


            return BookedSeats + PassengersAmount <= AvailableSeats;
            
        }

    }
}
