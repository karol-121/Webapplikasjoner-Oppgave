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

                var Departure1 = new DateTime(2021, 9, 20, 9, 30, 0);
                var Departure2 = new DateTime(2021, 9, 20, 19, 30, 0);
                var Departure3 = new DateTime(2021, 9, 22, 9, 30, 0);
                var Departure4 = new DateTime(2021, 9, 22, 19, 30, 0);
                var Departure5 = new DateTime(2021, 9, 23, 11, 00, 0);


                var Route1 = new Route { Origin = "Oslo", Destination = "Bergen" };
                var Route2 = new Route { Origin = "Bergen", Destination = "Oslo" };
                var Route3 = new Route { Origin = "Oslo", Destination = "Kiel" };
                var Route4 = new Route { Origin = "Kiel", Destination = "Oslo" };

                var Cruise1 = new Cruise { Route = Route1, Max_Passengers = 10, Passeger_Price = 100, Passegner_Underage_Price = 50, Pet_Price = 20, Vehicle_Price = 100 };
                var Cruise2 = new Cruise { Route = Route1, Max_Passengers = 7, Passeger_Price = 500, Passegner_Underage_Price = 200, Pet_Price = 100, Vehicle_Price = 1000 };
                var Cruise3 = new Cruise { Route = Route2, Max_Passengers = 10, Passeger_Price = 100, Passegner_Underage_Price = 50, Pet_Price = 20, Vehicle_Price = 100 };
                var Cruise4 = new Cruise { Route = Route2, Max_Passengers = 7, Passeger_Price = 500, Passegner_Underage_Price = 200, Pet_Price = 100, Vehicle_Price = 1000 };
                var Cruise5 = new Cruise { Route = Route3, Max_Passengers = 15, Passeger_Price = 300, Passegner_Underage_Price = 100, Pet_Price = 30, Vehicle_Price = 500 };
                var Cruise6 = new Cruise { Route = Route4, Max_Passengers = 15, Passeger_Price = 300, Passegner_Underage_Price = 100, Pet_Price = 30, Vehicle_Price = 500 };

                var Schedule1 = new Schedule { Cruise = Cruise1, Date = Departure1 };
                var Schedule2 = new Schedule { Cruise = Cruise2, Date = Departure1 };
                var Schedule3 = new Schedule { Cruise = Cruise1, Date = Departure2 };
                var Schedule4 = new Schedule { Cruise = Cruise2, Date = Departure2 };
                var Schedule5 = new Schedule { Cruise = Cruise1, Date = Departure3 };
                var Schedule6 = new Schedule { Cruise = Cruise2, Date = Departure3 };
                var Schedule7 = new Schedule { Cruise = Cruise1, Date = Departure4 };
                var Schedule8 = new Schedule { Cruise = Cruise2, Date = Departure4 };
                var Schedule9 = new Schedule { Cruise = Cruise5, Date = Departure5 };
                var Schedule10 = new Schedule { Cruise = Cruise6, Date = Departure5 };

                

                Context.Routes.Add(Route1);
                Context.Routes.Add(Route2);
                Context.Routes.Add(Route3);
                Context.Routes.Add(Route4);

                Context.Cruises.Add(Cruise1);
                Context.Cruises.Add(Cruise2);
                Context.Cruises.Add(Cruise3);
                Context.Cruises.Add(Cruise4);
                Context.Cruises.Add(Cruise5);
                Context.Cruises.Add(Cruise6);

                Context.Schedules.Add(Schedule1);
                Context.Schedules.Add(Schedule2);
                Context.Schedules.Add(Schedule3);
                Context.Schedules.Add(Schedule4);
                Context.Schedules.Add(Schedule5);
                Context.Schedules.Add(Schedule6);
                Context.Schedules.Add(Schedule7);
                Context.Schedules.Add(Schedule8);
                Context.Schedules.Add(Schedule9);
                Context.Schedules.Add(Schedule10);
                

                Context.SaveChanges();
            }
        }
    }
}
