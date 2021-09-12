using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IApplicationRepository
    {
        Task<List<Route>> GetRoutes(); 

        Task<List<Cruise>> FindCruises(int RouteId, int Departure_DayOfWeek); 

        Task<List<Cruise>> CheckAvailability(List<Cruise> Cruises, int PassengersAmount, DateTime DepartureDate); 


        //register order - den mest sannsynlighvis kommer å trenge register kunde if not og samme med poststed

        //register customer 

        //register post 
    }
}
