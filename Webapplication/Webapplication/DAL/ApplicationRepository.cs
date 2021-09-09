using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _DB;

        public ApplicationRepository(ApplicationDbContext DB)
        {
            _DB = DB;
        }

        public async Task<List<Route>> GetRoutes()
        {
            return await _DB.Routes.ToListAsync();
        }


    }
}
