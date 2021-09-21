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

        public async Task<List<Departure>> GetDepartures(int RouteId, string Date, int Passengers) 
        {
            //todo: validate passengers, return http response codes

            var Orginal_Date = DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture); //lager datetime objekt fra string parameter
            var To_Date = Orginal_Date.AddDays(3); //lager max-dato verdi
            var Interval = To_Date.Subtract(Orginal_Date); //danner et time span objekt som datetime trenger for å subtrakhere fra et gitt dato
            var From_Date = Orginal_Date.Subtract(Interval); //lager min-dato verdi

            
            var Departures = await _Local_DB.GetDepartures(RouteId, From_Date, To_Date); //henter alle utreiser i gitt intervall 
            return await _Local_DB.CheckAvailability(Departures, Passengers); //filtrerer og returnerer kun tilgjenglige utreiser
            
        }

        public async Task<ActionResult> RegisterOrder(OrderInformation OrderInformation)
        {
            try
            {
                //her skal man validere informasjon som ligger inn i objektet OrderInformation, også passenger amount
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
