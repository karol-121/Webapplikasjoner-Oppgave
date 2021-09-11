﻿using Microsoft.AspNetCore.Builder;
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
                var Cruise2 = new Cruise { Route = Route1, Departure_DayOfWeek = 1, Departure_Hour = 15, Departure_Minute = 00, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise3 = new Cruise { Route = Route1, Departure_DayOfWeek = 3, Departure_Hour = 9, Departure_Minute = 30, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise4 = new Cruise { Route = Route1, Departure_DayOfWeek = 3, Departure_Hour = 15, Departure_Minute = 00, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                
                var Cruise5 = new Cruise { Route = Route2, Departure_DayOfWeek = 1, Departure_Hour = 12, Departure_Minute = 00, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise6 = new Cruise { Route = Route2, Departure_DayOfWeek = 1, Departure_Hour = 20, Departure_Minute = 30, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise7 = new Cruise { Route = Route2, Departure_DayOfWeek = 3, Departure_Hour = 12, Departure_Minute = 00, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise8 = new Cruise { Route = Route2, Departure_DayOfWeek = 3, Departure_Hour = 20, Departure_Minute = 30, Max_Passengers = 300, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                
                var Cruise9 = new Cruise { Route = Route3, Departure_DayOfWeek = 2, Departure_Hour = 10, Departure_Minute = 30, Max_Passengers = 600, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                var Cruise10 = new Cruise { Route = Route3, Departure_DayOfWeek = 4, Departure_Hour = 10, Departure_Minute = 30, Max_Passengers = 600, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                
                var Cruise11 = new Cruise { Route = Route4, Departure_DayOfWeek = 2, Departure_Hour = 16, Departure_Minute = 30, Max_Passengers = 600, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                var Cruise12 = new Cruise { Route = Route4, Departure_DayOfWeek = 4, Departure_Hour = 16, Departure_Minute = 30, Max_Passengers = 600, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                

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
                Context.Cruises.Add(Cruise7);
                Context.Cruises.Add(Cruise8);
                Context.Cruises.Add(Cruise9);
                Context.Cruises.Add(Cruise10);
                Context.Cruises.Add(Cruise11);
                Context.Cruises.Add(Cruise12);


                Context.SaveChanges();
            }
        }
    }
}
