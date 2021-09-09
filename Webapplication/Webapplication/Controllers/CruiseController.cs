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
    public class CruiseController : ControllerBase
    {
        private readonly CruiseContext _DB;

        public CruiseController(CruiseContext DB)
        {
            _DB = DB;
        }

        public async Task<List<Cruise>> GetCruises()
        {
           return await _DB.Cruises.ToListAsync();
        }
    }
}
