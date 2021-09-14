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

        public async Task<List<Cruise>> FindCruises(int RouteId, int PassengerAmount, int Year, int Month, int Day)
        {
            DateTime date = new DateTime(Year, Month, Day);
            // find cruises
            List<Cruise> FoundCruises = await _Local_DB.FindCruises(RouteId, ((int)date.DayOfWeek));
            // check avaibility on found cruises
            // return avaible cruises
            return await _Local_DB.CheckAvailability(FoundCruises, PassengerAmount, date);
        }

        public async Task RegisterOrder(OrderInformation OrderInformation)
        {
            //validate the information in order information

            try
            {
                await _Local_DB.RegisterOrder(OrderInformation);
            } 
            catch (Exception e)
            {
                //for now just print message to console, but in future i want here to return the http code with the message.
                Console.WriteLine(e.Message);
            }

            
        }

        


    }
}
