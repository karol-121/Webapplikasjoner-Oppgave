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


                var Cruise1 = new Cruise { Route = Route1, Departure_DayOfWeek = 1, Departure_Hour = 9, Departure_Minute = 30, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };


                Context.Routes.Add(Route1);
                Context.Routes.Add(Route2);
                Context.Routes.Add(Route3);
                Context.Routes.Add(Route4);
                Context.Cruises.Add(Cruise1);


                Context.SaveChanges();
            }
        }
    }
}
