using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;
using Microsoft.EntityFrameworkCore;
using Webapplication.DAL;
using System.Globalization;

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

        public async Task<ActionResult<List<Departure>>> GetDepartures(int Route, string Date, int Passengers) 
        {
            
            
            //det er ikke så viktig at man kan få tilgang til biletter som er full booked, du kan ikke bestille dem uansett siden det sjekkes før registrering
            try
            {
                //todo, gjør slik at dersom date er gamlere enn dato i dag så set den til dagens dato.
                var Orginal_Date = DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter
                var To_Date = Orginal_Date.AddDays(3); //lager max-dato verdi
                var Interval = To_Date.Subtract(Orginal_Date); //danner et time span objekt som datetime trenger for å subtrahere fra et gitt dato
                var From_Date = Orginal_Date.Subtract(Interval); //lager min-dato verdi

                var Departures = await _Local_DB.GetDepartures(Route, From_Date, To_Date); //henter alle utreiser i gitt intervall 
                return await _Local_DB.CheckAvailability(Departures, Passengers); //filtrerer og returnerer kun tilgjenglige utreiser

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        public async Task<ActionResult> RegisterOrder(OrderInformation OrderInformation)
        {
            if (!ModelState.IsValid) //sjekkes om data send inn har riktig format som er definert i modellen.
            {
                return BadRequest("Wrong information format"); //hvis de ikke er riktige, return med en bad request response
            }

            try
            {
                await _Local_DB.RegisterOrder(OrderInformation); //prøve å registrere nye ordre
                return Ok("Ticket has been registered"); //returnere en ok http response status
                
                
            } 
            catch (Exception e)
            {
                // dersom det er noe feil ved registrering, kastes det exception som fanges her.
                return BadRequest(e.Message); // returnere en error http response med excepton melding.

            }

            
        }

        


    }
}
