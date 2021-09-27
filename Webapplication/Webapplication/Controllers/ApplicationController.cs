using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;
using Microsoft.EntityFrameworkCore;
using Webapplication.DAL;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Webapplication.Controllers
{
    [Route("API/[action]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _Local_DB;

        private ILogger<ApplicationController> _Local_Log;

        public ApplicationController(IApplicationRepository applicationRepository, ILogger<ApplicationController> logger)
        {
            _Local_DB = applicationRepository;
            _Local_Log = logger;
        }

        public async Task<List<Route>> GetRoutes()
        {
            _Local_Log.LogInformation("Requested routes");
            return await _Local_DB.GetRoutes(); //henter alle ruter som finnes i databasen
        }

        public async Task<ActionResult<List<Departure>>> GetDepartures(int Route, string From, string To, int Passengers) 
        {
            try
            {
                var From_Date = DateTime.ParseExact(From, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter
                var To_Date = DateTime.ParseExact(To, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter

                var Departures = await _Local_DB.GetDepartures(Route, From_Date, To_Date); //henter alle utreiser i gitt intervall 
                _Local_Log.LogInformation("Requested departures");
                return await _Local_DB.CheckAvailability(Departures, Passengers); //filtrerer og returnerer kun tilgjenglige utreiser

            } catch (Exception e)
            {
                _Local_Log.LogError(e.Message);
                return BadRequest(e.Message);
            }
            
        }

        public async Task<ActionResult> RegisterOrder(OrderInformation OrderInformation)
        {
            if (!ModelState.IsValid) //sjekkes om data send inn har riktig format som er definert i modellen.
            {
                _Local_Log.LogError("Information for order register are of invalid type");
                return BadRequest("Wrong information format"); //hvis de ikke er riktige, return med en bad request response
            }

            try
            {
                await _Local_DB.RegisterOrder(OrderInformation); //prøve å registrere nye ordre
                _Local_Log.LogInformation("Order has been registered");
                return Ok("Ticket has been registered"); //returnere en ok http response status
                
                
            } 
            catch (Exception e)
            {
                // dersom det er noe feil ved registrering, kastes det exception som fanges her.
                _Local_Log.LogError(e.Message);
                return BadRequest(e.Message); // returnere en error http response med excepton melding.

            }

            
        }

        


    }
}
