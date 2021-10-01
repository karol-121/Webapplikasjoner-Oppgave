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

                //dette her skal egentlig omgjøres dersom det har mistet integritet med excel
                //hvis noen skal oppdatere det er det best å undersøke alt grundig og gjenbruke så mye objekter som et er mulig
                //f.eks cruiser a - b og b - a kan bruke samme datoer, egentlig så skulle rute a - b og b - a bruke samme cruise men whatever

                //Routes
                var Route1 = new Route { Origin = "Larvik", Destination = "Hirtshals", Return_id = 2 };
                var Route2 = new Route { Origin = "Hirtshals", Destination = "Larvik", Return_id = 1 };
                var Route3 = new Route { Origin = "Bodø", Destination = "Moskenes", Return_id = 4 };
                var Route4 = new Route { Origin = "Moskenes", Destination = "Bodø", Return_id = 3 };
                var Route5 = new Route { Origin = "Stavanger", Destination = "Bergen", Return_id = 6 };
                var Route6 = new Route { Origin = "Bergen", Destination = "Stavanger", Return_id = 5 };

                //Cruise1 
                var Departure1 = new DateTime(2021, 10, 4, 8, 00, 0);
                var Departure2 = new DateTime(2021, 10, 4, 17, 00, 0);

                var Departure3 = new DateTime(2021, 10, 6, 8, 00, 0);
                var Departure4 = new DateTime(2021, 10, 6, 17, 00, 0);

                var Departure5 = new DateTime(2021, 10, 8, 8, 00, 0);
                var Departure6 = new DateTime(2021, 10, 8, 17, 00, 0);

                //Cruise2
                var Departure7 = new DateTime(2021, 10, 4, 14, 00, 0);
                var Departure8 = new DateTime(2021, 10, 8, 14, 00, 0);
                var Departure9 = new DateTime(2021, 10, 9, 14, 00, 0);
                var Departure10 = new DateTime(2021, 10, 10, 14, 00, 0);

                //Cruise3
                var Departure11 = new DateTime(2021, 10, 4, 12, 00, 0);
                var Departure12 = new DateTime(2021, 10, 4, 22, 00, 0);
                
                var Departure13 = new DateTime(2021, 10, 7, 12, 00, 0);
                var Departure14 = new DateTime(2021, 10, 7, 22, 00, 0);
                
                var Departure15 = new DateTime(2021, 10, 9, 12, 00, 0);
                var Departure16 = new DateTime(2021, 10, 9, 22, 00, 0);

                //Cruise4
                var Departure17 = new DateTime(2021, 10, 4, 9, 00, 0);
                var Departure18 = new DateTime(2021, 10, 4, 16, 00, 0);
                
                var Departure19 = new DateTime(2021, 10, 6, 9, 00, 0);
                var Departure20 = new DateTime(2021, 10, 6, 16, 00, 0);
                
                var Departure21 = new DateTime(2021, 10, 8, 9, 00, 0);
                var Departure22 = new DateTime(2021, 10, 8, 16, 00, 0);
                
                var Departure23 = new DateTime(2021, 10, 10, 9, 00, 0);
                var Departure24 = new DateTime(2021, 10, 10, 16, 00, 0);

                //Cruise5
                var Departure25 = new DateTime(2021, 10, 4, 7, 00, 0);
                var Departure26 = new DateTime(2021, 10, 4, 15, 00, 0);
                
                var Departure27 = new DateTime(2021, 10, 5, 7, 00, 0);
                var Departure28 = new DateTime(2021, 10, 5, 15, 00, 0);
                
                var Departure29 = new DateTime(2021, 10, 8, 7, 00, 0);
                var Departure30 = new DateTime(2021, 10, 8, 15, 00, 0);
                
                var Departure31 = new DateTime(2021, 10, 7, 7, 00, 0);
                var Departure32 = new DateTime(2021, 10, 7, 15, 00, 0);
                
                var Departure33 = new DateTime(2021, 10, 8, 7, 00, 0);
                var Departure34 = new DateTime(2021, 10, 8, 15, 00, 0);

                //Cruise6
                var Departure35 = new DateTime(2021, 10, 5, 14, 00, 0);
                var Departure36 = new DateTime(2021, 10, 7, 14, 00, 0);
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
