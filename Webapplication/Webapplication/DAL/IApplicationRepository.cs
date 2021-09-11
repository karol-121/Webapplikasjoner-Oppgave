using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IApplicationRepository
    {
        Task<List<Route>> GetRoutes(); //henter alle ruter som finnes i databasen

        Task<List<Cruise>> FindCruises(int RouteId, int Departure_DayOfWeek); //returnerer alle cruiser som stemmer med gitt parameterene


        //check avaiblility - best at den teller opp antall ordrer med gitt cruise id og dato

        //register order - den mest sannsynlighvis kommer å trenge register kunde if not og samme med poststed

        //register customer 

        //register post 
    }
}
