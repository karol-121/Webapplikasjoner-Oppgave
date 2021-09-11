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

        //find cruises - best at den tar inn en parameter array slik at den kan være universielt. Hva som lettes etter bestemmer i controlleren

        //check avaiblility - best at den teller opp antall ordrer med gitt cruise id og dato

        //register order - den mest sannsynlighvis kommer å trenge register kunde if not og samme med poststed

        //register customer 

        //register post 
    }
}
