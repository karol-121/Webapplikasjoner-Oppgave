using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;
using Microsoft.EntityFrameworkCore;
using Webapplication.DAL;

namespace Webapplication.Controllers
{
    [Route("API/[action]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _Local_DB;

        public ApplicationController(IApplicationRepository applicationRepository)
        {
            _Local_DB = applicationRepository;
        }

        public async Task<List<Route>> GetRoutes()
        {
            return await _Local_DB.GetRoutes(); //henter alle ruter som finnes i databasen
        }

        public async Task<List<Departure>> GetDepartures() //skal hente alle mulige cruiser med bestemt route og dato som er ikke fulle i guess?
        {
            throw new NotImplementedException();
            
        }

        /*public async Task<List<Cruise>> FindCruises(int RouteId, int PassengerAmount, int Year, int Month, int Day) //her tenker jeg om endre dette til string
        {
            DateTime Date = new DateTime(Year, Month, Day); //Dette er ikke nødvendig, man kunne passere datetime objekt som parameter,
                                                            //men jeg vil beholde denne metoden "get friendly" slik at ingen objekt skal inn

            List<Cruise> FoundCruises = await _Local_DB.FindCruises(RouteId, Date); //henter alle mulige cruiser

            return await _Local_DB.CheckAvailability(FoundCruises, PassengerAmount, Date); //sjekker of forkaster disse cruiser som har ikke nok plass/plasser
        }*/

        public async Task<ActionResult> RegisterOrder(OrderInformation OrderInformation)
        {
            try
            {
                //her skal man validere informasjon som ligger inn i objektet OrderInformation
                await _Local_DB.RegisterOrder(OrderInformation); //prøve å registrere nye ordre
                return Ok(); //returnere en ok http response status
                
            } 
            catch (Exception e)
            {
                // dersom det er noe feil ved registrering, kastes det exception som fanges her.
                return BadRequest(e.Message); // returnere en error http response med excepton melding.

            }

            
        }

        


    }
}
