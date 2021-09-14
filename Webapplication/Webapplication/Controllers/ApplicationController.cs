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
            return await _Local_DB.GetRoutes();
        }

        public async Task<List<Cruise>> FindCruises(int RouteId, int PassengerAmount, int year, int month, int day) //her endre til å ta imot dato objekt og så avgjøre day of the week derfra. Samtidig bruk dato til å sjekke tilgjengelighet.
        {
            DateTime date = new DateTime(year, month, day);
            // find cruises
            List<Cruise> FoundCruises = await _Local_DB.FindCruises(RouteId, ((int)date.DayOfWeek));
            // check avaibility on found cruises
            // return avaible cruises
            return await _Local_DB.CheckAvailability(FoundCruises, PassengerAmount, date);
        }

        public async Task RegisterOrder(OrderInformation orderInformation)
        {
            //validate the information in order information

            Console.WriteLine("the object has been delivered");

            await _Local_DB.RegisterOrder(orderInformation);
        }

        


    }
}
