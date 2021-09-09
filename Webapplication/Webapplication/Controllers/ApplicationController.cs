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
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _DB;

        public ApplicationController(ApplicationDbContext DB)
        {
            _DB = DB;
        }

        public async Task<List<Cruise>> GetCruises()
        {
           return await _DB.Cruises.ToListAsync();
        }

        public string DebugString()
        {
            return "this is test";
        }
    }
}
