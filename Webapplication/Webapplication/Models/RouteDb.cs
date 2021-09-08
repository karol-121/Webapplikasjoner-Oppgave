using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class RouteDb : DbContext
    {
        public RouteDb (DbContextOptions<RouteDb> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Route> Routes { get; set; }
    }
}
