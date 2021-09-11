using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class DBinit
    {
        public static void InitializeApplicationDB(IApplicationBuilder Application)
        {
            using (var ServiceScope = Application.ApplicationServices.CreateScope())
            {
                var Context = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>();

                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();

                var Route1 = new Route { Origin = "Oslo", Destination = "Bergen" };
                var Route2 = new Route { Origin = "Bergen", Destination = "Oslo" };
                var Route3 = new Route { Origin = "Oslo", Destination = "Kiel" };
                var Route4 = new Route { Origin = "Kiel", Destination = "Oslo" };

                Context.Routes.Add(Route1);
                Context.Routes.Add(Route2);
                Context.Routes.Add(Route3);
                Context.Routes.Add(Route4);

                Context.SaveChanges();
            }
        }
    }
}
