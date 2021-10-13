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
using Microsoft.AspNetCore.Http;

namespace Webapplication.Controllers
{
    [Route("API/[action]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _Local_DB; //database objekt

        private ILogger<ApplicationController> _Local_Log; //log objekt
        
        public ApplicationController(IApplicationRepository applicationRepository, ILogger<ApplicationController> logger)
        {
            _Local_DB = applicationRepository;
            _Local_Log = logger;
        }

        //summary: henter alle ruter som finnes
        //returns: liste med rute objekter
        public async Task<ActionResult> GetRoutes()
        {
            var Routes = await _Local_DB.GetRoutes(); //henter alle ruter som finnes i databasen
            _Local_Log.LogInformation("Requested routes."); //logge informasjon om forespørsel
            return Ok(Routes); //returnere liste med http ok response status
        }

        //summary: henter alle samsvarende departures bestemt av parameters
        //parameters: int Route - bestemmer ruten til departures, string From - dato string i format yyyy-MM-dd som bestemmer fra og med dato,
        //string To - dato string i format yyyy-MM-dd som bestemmer til og med dato, int Passengers - antall personer som det skal være plass for
        //returns: liste med samsvarende departure objekter, http Bad request - dersom parameter/parameterene er invalid
        public async Task<ActionResult> GetDepartures(int Route, string From, string To, int Passengers) 
        {
            try
            {
                var From_Date = DateTime.ParseExact(From, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter
                var To_Date = DateTime.ParseExact(To, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter
                var Today = DateTime.Now; //nåvarende dato

                if (From_Date.Date == Today.Date) //dersom intervalet starter på dagens dato, legg til nåvarende tid, slik at det vises ikke utreiser fra i dag som har gått.
                {
                    From_Date = From_Date.AddHours(Today.Hour);
                    From_Date = From_Date.AddMinutes(Today.Minute);
                }

                var Departures = await _Local_DB.GetDepartures(Route, From_Date, To_Date); //henter alle utreiser i gitt intervall 
                var AvailableDep = await _Local_DB.CheckAvailability(Departures, Passengers); //filtrerer og returnerer kun tilgjenglige utreiser
                
                _Local_Log.LogInformation("Requested departures for route "+Route+", from "+From_Date+" to "+To_Date+" for "+Passengers+" passenger(s)."); //logge informasjon om forespørsel
                return Ok(AvailableDep); //returnere liste med http ok response status
            } 
            catch (Exception e)
            {
                _Local_Log.LogError("Exception thrown while requesting departures: "+e.Message); //logge feilmelding
                return BadRequest(e.Message); //returnere en http bad request response
            }
            
        }

        //summary: validerer informasjon og registrerer ordre
        //parameters: Order order - objekt som holder informasjon om ordre
        //returns: http Ok - ved vellykket registrering, http Bad request - dersom informasjon er invalid
        public async Task<ActionResult> RegisterOrder(Order order)
        {

            if (!ModelState.IsValid) //sjekkes om data send inn har riktig format som er definert i modellen.
            {
                _Local_Log.LogError("Information for order register are of invalid type");
                return BadRequest("Wrong information format"); //hvis de ikke er riktige, return med en bad request response
            }

            string sessionKey = DateTime.Now.ToString();    //generering session key som i dette tilfelle er nå dato
                                                            //den kobler alle items som skal registreres ved å signere hver bilett med unik session id.

            try
            {
                foreach (var orderItem in order.Items) //registreres alle biletter i ordre
                {
                    await _Local_DB.RegisterOrderItem(orderItem, sessionKey); //prøve å registrere ny bilett
                    _Local_Log.LogInformation("Order has been registered."); //logger informasjon om vellykket registrering
                }

                return Ok("Ticket/tickets has been registered"); //returnerer en ok http response status
            } 
            catch (Exception e)
            {
                
                _Local_Log.LogError("Exception thrown while order registration: "+e.Message); //logge feilmelding
                await _Local_DB.RemoveSessionTickets(sessionKey); //fjerne alle andre biletter som har blitt registrert tidligere inn i denne sesjon.
                return BadRequest(e.Message); // returnere en error http response med excepton melding.
            }
        }

    }
}
