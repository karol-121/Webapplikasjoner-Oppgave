using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;
using Microsoft.EntityFrameworkCore;

namespace Webapplication.Controllers
{
    [Route("[controller]/[action]")]
    public class RouteController : ControllerBase
    {
        private readonly RouteContext _DB;

        public RouteController(RouteContext DB)
        {
            _DB = DB;
        }

        public async Task<List<Route>> GetRoutes()
        {
           return await _DB.Routes.ToListAsync();
        }
    }
}
