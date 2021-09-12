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

                var Post1 = new Post { Zip_Code = "1224", City = "Bergen" };

                var Customer1 = new Customer { Name = "Per", Surname = "Hansen", Age = 34, Address = "Hovedveien 3", Post = Post1, Phone = 12345678, Email = "Per@email.com" };

                var Date1 = new DateTime(2021, 9, 13);
                var Date2 = new DateTime(2021, 9, 12);


                var Cruise1 = new Cruise { Route = Route1, Departure_DayOfWeek = 1, Departure_Hour = 9, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise2 = new Cruise { Route = Route1, Departure_DayOfWeek = 1, Departure_Hour = 15, Departure_Minute = 00, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise3 = new Cruise { Route = Route1, Departure_DayOfWeek = 3, Departure_Hour = 9, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise4 = new Cruise { Route = Route1, Departure_DayOfWeek = 3, Departure_Hour = 15, Departure_Minute = 00, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                
                var Cruise5 = new Cruise { Route = Route2, Departure_DayOfWeek = 1, Departure_Hour = 12, Departure_Minute = 00, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise6 = new Cruise { Route = Route2, Departure_DayOfWeek = 1, Departure_Hour = 20, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise7 = new Cruise { Route = Route2, Departure_DayOfWeek = 3, Departure_Hour = 12, Departure_Minute = 00, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                var Cruise8 = new Cruise { Route = Route2, Departure_DayOfWeek = 3, Departure_Hour = 20, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 0 };
                
                var Cruise9 = new Cruise { Route = Route3, Departure_DayOfWeek = 2, Departure_Hour = 10, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                var Cruise10 = new Cruise { Route = Route3, Departure_DayOfWeek = 4, Departure_Hour = 10, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                
                var Cruise11 = new Cruise { Route = Route4, Departure_DayOfWeek = 2, Departure_Hour = 16, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };
                var Cruise12 = new Cruise { Route = Route4, Departure_DayOfWeek = 4, Departure_Hour = 16, Departure_Minute = 30, Max_Passengers = 10, Passeger_Price = 500, Passegner_Underage_Price = 350, Pet_Price = 50, Vehicle_Price = 200 };

                var Order1 = new Order { Order_Date = Date2, Customer = Customer1, Cruise = Cruise5, Cruise_Date = Date1, Passengers = 1, Passenger_Underage = 0, Pets = 1, Vehicles = 1};
                

                Context.Routes.Add(Route1);
                Context.Routes.Add(Route2);
                Context.Routes.Add(Route3);
                Context.Routes.Add(Route4);

                Context.Posts.Add(Post1);
                Context.Customers.Add(Customer1);
                
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

                Context.Orders.Add(Order1);


                Context.SaveChanges();
            }
        }
    }
}
