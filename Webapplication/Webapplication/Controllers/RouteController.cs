using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.Controllers
{
    [Route("[controller]/[action]")]
    public class RouteController : ControllerBase
    {

        public List<Route> GetRoutes()
        {
            var Routes = new List<Route>();
            
            var a = new Route();
            a.Origin = "Oslo";
            a.Destination = "Kiel";

            var b = new Route();
            b.Origin = "Kiel";
            b.Destination = "Stockholm";

            Routes.Add(a);
            Routes.Add(b);

            return Routes;
        }
    }
}
