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

                //todo: add dato og legg til data for neste 2 forkommende uker.

                //Routes
                var san_str = new Route { Origin = "Sandefjord", Destination = "Strømstad", Return_id = 2 };
                var str_san = new Route { Origin = "Strømstad", Destination = "Sandefjord", Return_id = 1 };
                var stav_berg = new Route { Origin = "Stavanger", Destination = "Bergen", Return_id = 4 };
                var berg_stav = new Route { Origin = "Bergen", Destination = "Stavanger", Return_id = 3 };
                var osl_kie = new Route { Origin = "Oslo", Destination = "Kiel", Return_id = 6 };
                var kie_osl = new Route { Origin = "Kiel", Destination = "Oslo", Return_id = 5 };

                //Uke 41
                //Cruise1
                var mon_41_8 = new DateTime(2021, 10, 11, 8, 00, 0);
                var mon_41_17 = new DateTime(2021, 10, 11, 17, 00, 0);

                var wen_41_8 = new DateTime(2021, 10, 13, 8, 00, 0);
                var wen_41_17 = new DateTime(2021, 10, 13, 17, 00, 0);

                var fri_41_8 = new DateTime(2021, 10, 15, 8, 00, 0);
                var fri_41_17 = new DateTime(2021, 10, 15, 17, 00, 0);

                //Cruise2
                var tue_41_8 = new DateTime(2021, 10, 12, 8, 00, 0);
                var tue_41_15 = new DateTime(2021, 10, 12, 15, 00, 0);

                var thu_41_8 = new DateTime(2021, 10, 14, 8, 00, 0);
                var thu_41_15 = new DateTime(2021, 10, 14, 15, 00, 0);

                //Cruise3
                var sat_41_10 = new DateTime(2021, 10, 16, 10, 00, 0);
                
                //todo: øke antall maks plasser i produksjon, ellers brukes det små tall for å kunne teste.

                var Cruise1 = new Cruise { Route = san_str, Max_Passengers = 10, Passeger_Price = 449, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 99 };
                var Cruise1_rev = new Cruise { Route = str_san, Max_Passengers = 10, Passeger_Price = 449, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 99 };

                var Cruise2 = new Cruise { Route = stav_berg, Max_Passengers = 10, Passeger_Price = 549, Passegner_Underage_Price = 399, Pet_Price = 100, Vehicle_Price = 149 };
                var Cruise2_rev = new Cruise { Route = berg_stav, Max_Passengers = 10, Passeger_Price = 549, Passegner_Underage_Price = 399, Pet_Price = 100, Vehicle_Price = 149 };

                var Cruise3 = new Cruise { Route = osl_kie, Max_Passengers = 10, Passeger_Price = 749, Passegner_Underage_Price = 549, Pet_Price = 100, Vehicle_Price = 200 };
                var Cruise3_rev = new Cruise { Route = kie_osl, Max_Passengers = 10, Passeger_Price = 749, Passegner_Underage_Price = 549, Pet_Price = 100, Vehicle_Price = 200 };

    
                //Cruise1
                var Schedule1 = new Departure { Cruise = Cruise1, Date = mon_41_8 };
                var Schedule2 = new Departure { Cruise = Cruise1, Date = mon_41_17 };
                var Schedule3 = new Departure { Cruise = Cruise1, Date = wen_41_8 };
                var Schedule4 = new Departure { Cruise = Cruise1, Date = wen_41_17 };
                var Schedule5 = new Departure { Cruise = Cruise1, Date = fri_41_8 };
                var Schedule6 = new Departure { Cruise = Cruise1, Date = fri_41_17 };

                //Cruise1 reverse
                var Schedule7 = new Departure { Cruise = Cruise1_rev, Date = mon_41_8 };
                var Schedule8 = new Departure { Cruise = Cruise1_rev, Date = mon_41_17 };
                var Schedule9 = new Departure { Cruise = Cruise1_rev, Date = wen_41_8 };
                var Schedule10 = new Departure { Cruise = Cruise1_rev, Date = wen_41_17 };
                var Schedule11 = new Departure { Cruise = Cruise1_rev, Date = fri_41_8 };
                var Schedule12 = new Departure { Cruise = Cruise1_rev, Date = fri_41_17 };

                //Cruise2
                var Schedule13 = new Departure { Cruise = Cruise2, Date = tue_41_8 };
                var Schedule14 = new Departure { Cruise = Cruise2, Date = tue_41_15 };
                var Schedule15 = new Departure { Cruise = Cruise2, Date = thu_41_8 };
                var Schedule16 = new Departure { Cruise = Cruise2, Date = thu_41_15 };

                //Cruisea reverse
                var Schedule17 = new Departure { Cruise = Cruise2_rev, Date = tue_41_8 };
                var Schedule18 = new Departure { Cruise = Cruise2_rev, Date = tue_41_15 };
                var Schedule19 = new Departure { Cruise = Cruise2_rev, Date = thu_41_8 };
                var Schedule20 = new Departure { Cruise = Cruise2_rev, Date = thu_41_15 };

                //Cruise3
                var Schedule21 = new Departure { Cruise = Cruise3, Date = sat_41_10 };

                //Cruise3 reverse
                var Schedule22 = new Departure { Cruise = Cruise3_rev, Date = sat_41_10 };


                //routes som skal inn i databasen

                Context.Routes.Add(san_str);
                Context.Routes.Add(str_san);
                Context.Routes.Add(stav_berg);
                Context.Routes.Add(berg_stav);
                Context.Routes.Add(osl_kie);
                Context.Routes.Add(kie_osl);

                //cruises som skal inn i databasen

                Context.Cruises.Add(Cruise1);
                Context.Cruises.Add(Cruise1_rev);
                Context.Cruises.Add(Cruise2);
                Context.Cruises.Add(Cruise2_rev);
                Context.Cruises.Add(Cruise3);
                Context.Cruises.Add(Cruise3_rev);

                //departures skal inn i databasen

                //cruise 1
                Context.Departures.Add(Schedule1);
                Context.Departures.Add(Schedule2);
                Context.Departures.Add(Schedule3);
                Context.Departures.Add(Schedule4);
                Context.Departures.Add(Schedule5);
                Context.Departures.Add(Schedule6);

                //cruise 1 reverse
                Context.Departures.Add(Schedule7);
                Context.Departures.Add(Schedule8);
                Context.Departures.Add(Schedule9);
                Context.Departures.Add(Schedule10);
                Context.Departures.Add(Schedule11);
                Context.Departures.Add(Schedule12);

                //cruise 2
                Context.Departures.Add(Schedule13);
                Context.Departures.Add(Schedule14);
                Context.Departures.Add(Schedule15);
                Context.Departures.Add(Schedule16);

                //cruise 2 reverse
                Context.Departures.Add(Schedule17);
                Context.Departures.Add(Schedule18);
                Context.Departures.Add(Schedule19);
                Context.Departures.Add(Schedule20);

                //cruise 3 
                Context.Departures.Add(Schedule21);

                //cruise 3 reverse
                Context.Departures.Add(Schedule22);


                Context.SaveChanges();
            }
        }
    }
}
