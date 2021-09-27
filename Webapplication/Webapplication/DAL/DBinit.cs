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

                //Routes
                var Route1 = new Route { Origin = "Larvik", Destination = "Hirtshals" };
                var Route2 = new Route { Origin = "Oslo", Destination = "København" };
                var Route3 = new Route { Origin = "Bodø", Destination = "Moskenes" };
                var Route4 = new Route { Origin = "Sandefjord", Destination = "Strømstad" };
                var Route5 = new Route { Origin = "Stavanger", Destination = "Bergen" };
                var Route6 = new Route { Origin = "Oslo", Destination = "Kiel" };

                //Cruise1 
                var Departure1 = new DateTime(2021, 9, 27, 8, 00, 0);
                var Departure2 = new DateTime(2021, 9, 27, 17, 00, 0);

                var Departure3 = new DateTime(2021, 9, 29, 8, 00, 0);
                var Departure4 = new DateTime(2021, 9, 29, 17, 00, 0);

                var Departure5 = new DateTime(2021, 10, 1, 8, 00, 0);
                var Departure6 = new DateTime(2021, 10, 1, 17, 00, 0);

                //Cruise2
                var Departure7 = new DateTime(2021, 9, 27, 14, 00, 0);
                var Departure8 = new DateTime(2021, 10, 1, 14, 00, 0);
                var Departure9 = new DateTime(2021, 10, 2, 14, 00, 0);
                var Departure10 = new DateTime(2021, 10, 3, 14, 00, 0);

                //Cruise3
                var Departure11 = new DateTime(2021, 9, 27, 12, 00, 0);
                var Departure12 = new DateTime(2021, 9, 27, 22, 00, 0);
                
                var Departure13 = new DateTime(2021, 9, 30, 12, 00, 0);
                var Departure14 = new DateTime(2021, 9, 30, 22, 00, 0);
                
                var Departure15 = new DateTime(2021, 10, 2, 12, 00, 0);
                var Departure16 = new DateTime(2021, 10, 2, 22, 00, 0);

                //Cruise4
                var Departure17 = new DateTime(2021, 9, 27, 9, 00, 0);
                var Departure18 = new DateTime(2021, 9, 27, 16, 00, 0);
                
                var Departure19 = new DateTime(2021, 9, 29, 9, 00, 0);
                var Departure20 = new DateTime(2021, 9, 29, 16, 00, 0);
                
                var Departure21 = new DateTime(2021, 10, 1, 9, 00, 0);
                var Departure22 = new DateTime(2021, 10, 1, 16, 00, 0);
                
                var Departure23 = new DateTime(2021, 10, 3, 9, 00, 0);
                var Departure24 = new DateTime(2021, 10, 3, 16, 00, 0);

                //Cruise5
                var Departure25 = new DateTime(2021, 9, 27, 7, 00, 0);
                var Departure26 = new DateTime(2021, 9, 27, 15, 00, 0);
                
                var Departure27 = new DateTime(2021, 9, 28, 7, 00, 0);
                var Departure28 = new DateTime(2021, 9, 28, 15, 00, 0);
                
                var Departure29 = new DateTime(2021, 9, 29, 7, 00, 0);
                var Departure30 = new DateTime(2021, 9, 29, 15, 00, 0);
                
                var Departure31 = new DateTime(2021, 9, 30, 7, 00, 0);
                var Departure32 = new DateTime(2021, 9, 30, 15, 00, 0);
                
                var Departure33 = new DateTime(2021, 10, 1, 7, 00, 0);
                var Departure34 = new DateTime(2021, 10, 1, 15, 00, 0);

                //Cruise6
                var Departure35 = new DateTime(2021, 9, 28, 14, 00, 0);
                var Departure36 = new DateTime(2021, 9, 30, 14, 00, 0);
                //Cruise2 - departure9, dato er lik, samtidig det er eneste dato som gjentar seg fordi blerton har gjort bra jobb med unikate utreiser :)
           

                //todo: endre antall passengers etterhvert, det er enklere å teste full booking med lavere total nå
                //todo: legg til noe cruiser som bruker samme route for å gjøre separate routes mer hensiktmessing
                var Cruise1 = new Cruise { Route = Route1, Max_Passengers = 10, Passeger_Price = 549, Passegner_Underage_Price = 399, Pet_Price = 100, Vehicle_Price = 149 };
                var Cruise2 = new Cruise { Route = Route2, Max_Passengers = 10, Passeger_Price = 499, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 149 };
                var Cruise3 = new Cruise { Route = Route3, Max_Passengers = 10, Passeger_Price = 749, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 0 };
                var Cruise4 = new Cruise { Route = Route4, Max_Passengers = 10, Passeger_Price = 399, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 99 };
                var Cruise5 = new Cruise { Route = Route5, Max_Passengers = 10, Passeger_Price = 499, Passegner_Underage_Price = 349, Pet_Price = 100, Vehicle_Price = 199 };
                var Cruise6 = new Cruise { Route = Route6, Max_Passengers = 10, Passeger_Price = 399, Passegner_Underage_Price = 349, Pet_Price = 100, Vehicle_Price = 299 };

    
                //Cruise1
                var Schedule1 = new Departure { Cruise = Cruise1, Date = Departure1 };
                var Schedule2 = new Departure { Cruise = Cruise1, Date = Departure2 };
                var Schedule3 = new Departure { Cruise = Cruise1, Date = Departure3 };
                var Schedule4 = new Departure { Cruise = Cruise1, Date = Departure4 };
                var Schedule5 = new Departure { Cruise = Cruise1, Date = Departure5 };
                var Schedule6 = new Departure { Cruise = Cruise1, Date = Departure6 };

                //Cruise2
                var Schedule7 = new Departure { Cruise = Cruise2, Date = Departure7 };
                var Schedule8 = new Departure { Cruise = Cruise2, Date = Departure8 };
                var Schedule9 = new Departure { Cruise = Cruise2, Date = Departure9 };
                var Schedule10 = new Departure { Cruise = Cruise2, Date = Departure10 };

                //Cruise3
                var Schedule11 = new Departure { Cruise = Cruise3, Date = Departure11 };
                var Schedule12 = new Departure { Cruise = Cruise3, Date = Departure12 };
                var Schedule13 = new Departure { Cruise = Cruise3, Date = Departure13 };
                var Schedule14 = new Departure { Cruise = Cruise3, Date = Departure14 };
                var Schedule15 = new Departure { Cruise = Cruise3, Date = Departure15 };
                var Schedule16 = new Departure { Cruise = Cruise3, Date = Departure16 };

                //Cruise4
                var Schedule17 = new Departure { Cruise = Cruise4, Date = Departure17 };
                var Schedule18 = new Departure { Cruise = Cruise4, Date = Departure18 };
                var Schedule19 = new Departure { Cruise = Cruise4, Date = Departure19 };
                var Schedule20 = new Departure { Cruise = Cruise4, Date = Departure20 };
                var Schedule21 = new Departure { Cruise = Cruise4, Date = Departure21 };
                var Schedule22 = new Departure { Cruise = Cruise4, Date = Departure22 };
                var Schedule23 = new Departure { Cruise = Cruise4, Date = Departure23 };
                var Schedule24 = new Departure { Cruise = Cruise4, Date = Departure24 };

                //Cruise5
                var Schedule25 = new Departure { Cruise = Cruise5, Date = Departure25 };
                var Schedule26 = new Departure { Cruise = Cruise5, Date = Departure26 };
                var Schedule27 = new Departure { Cruise = Cruise5, Date = Departure27 };
                var Schedule28 = new Departure { Cruise = Cruise5, Date = Departure28 };
                var Schedule29 = new Departure { Cruise = Cruise5, Date = Departure29 };
                var Schedule30 = new Departure { Cruise = Cruise5, Date = Departure30 };
                var Schedule31 = new Departure { Cruise = Cruise5, Date = Departure31 };
                var Schedule32 = new Departure { Cruise = Cruise5, Date = Departure32 };
                var Schedule33 = new Departure { Cruise = Cruise5, Date = Departure33 };
                var Schedule34 = new Departure { Cruise = Cruise5, Date = Departure34 };

                //Cruise6
                var Schedule35 = new Departure { Cruise = Cruise6, Date = Departure35 };
                var Schedule36 = new Departure { Cruise = Cruise6, Date = Departure36 };
                var Schedule37 = new Departure { Cruise = Cruise6, Date = Departure9 };

                Context.Routes.Add(Route1);
                Context.Routes.Add(Route2);
                Context.Routes.Add(Route3);
                Context.Routes.Add(Route4);
                Context.Routes.Add(Route5);
                Context.Routes.Add(Route6);

                //Departures skal ikke inn i databasen dersom de er kun hjelpe metoder her, men burde de kanskje?

                Context.Cruises.Add(Cruise1);
                Context.Cruises.Add(Cruise2);
                Context.Cruises.Add(Cruise3);
                Context.Cruises.Add(Cruise4);
                Context.Cruises.Add(Cruise5);
                Context.Cruises.Add(Cruise6);

                //Cruise1
                Context.Departures.Add(Schedule1);
                Context.Departures.Add(Schedule2);
                Context.Departures.Add(Schedule3);
                Context.Departures.Add(Schedule4);
                Context.Departures.Add(Schedule5);
                Context.Departures.Add(Schedule6);
                
                //Cruise2
                Context.Departures.Add(Schedule7);
                Context.Departures.Add(Schedule8);
                Context.Departures.Add(Schedule9);
                Context.Departures.Add(Schedule10);
                
                //Cruise3
                Context.Departures.Add(Schedule11);
                Context.Departures.Add(Schedule12);
                Context.Departures.Add(Schedule13);
                Context.Departures.Add(Schedule14);
                Context.Departures.Add(Schedule15);
                Context.Departures.Add(Schedule16);
                
                //Cruise4
                Context.Departures.Add(Schedule17);
                Context.Departures.Add(Schedule18);
                Context.Departures.Add(Schedule19);
                Context.Departures.Add(Schedule20);
                Context.Departures.Add(Schedule21);
                Context.Departures.Add(Schedule22);
                Context.Departures.Add(Schedule23);
                Context.Departures.Add(Schedule24);
                Context.Departures.Add(Schedule25);
                
                //Cruise5
                Context.Departures.Add(Schedule26);
                Context.Departures.Add(Schedule27);
                Context.Departures.Add(Schedule28);
                Context.Departures.Add(Schedule29);
                Context.Departures.Add(Schedule30);
                Context.Departures.Add(Schedule31);
                Context.Departures.Add(Schedule32);
                Context.Departures.Add(Schedule33);
                Context.Departures.Add(Schedule34);
                
                //Cruise6
                Context.Departures.Add(Schedule35);
                Context.Departures.Add(Schedule36);
                Context.Departures.Add(Schedule37);

                Context.SaveChanges();
            }
        }
    }
}
