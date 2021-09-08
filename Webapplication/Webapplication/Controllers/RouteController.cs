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
        private readonly RouteDb _routeDb;

        public RouteController(RouteDb routeDb)
        {
            _routeDb = routeDb;
        }

        public List<Route> GetRoutes()
        {
            return _routeDb.Routes.ToList();
        }
    }
}
