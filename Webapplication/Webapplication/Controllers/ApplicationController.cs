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

        public async Task<List<Cruise>> FindCruises(int RouteId, int PassengerAmount, int Year, int Month, int Day) //her tenker jeg om endre dette til string
        {
            DateTime Date = new DateTime(Year, Month, Day); //Dette er ikke nødvendig, man kunne passere datetime objekt som parameter,
                                                            //men jeg vil beholde denne metoden "get friendly" slik at ingen objekt skal inn

            List<Cruise> FoundCruises = await _Local_DB.FindCruises(RouteId, Date); //henter alle mulige cruiser

            return await _Local_DB.CheckAvailability(FoundCruises, PassengerAmount, Date); //sjekker of forkaster disse cruiser som har ikke nok plass/plasser
        }

        public async Task RegisterOrder(OrderInformation OrderInformation)
        {
            //her skal man validere informasjon som ligger inn i objektet OrderInformation
            try
            {
                await _Local_DB.RegisterOrder(OrderInformation); //prøve å registrere nye ordre
            } 
            catch (Exception e)
            {
                // dersom det er noe feil ved registrering, kastes det exception som fanges her.
                // for nå skrives det kun meldig til consolen, men her skal det returneres en http kode, informasjon skal også tilbake til klienten.
                Console.WriteLine(e.Message);
            }

            
        }

        


    }
}
