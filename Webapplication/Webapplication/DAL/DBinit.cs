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

                //DateTime currentDate = new DateTime(2021, 10, 1, 0, 0, 0);
                DateTime currentDate = DateTime.Today; //nå tid

                int dayOfWeek = (int)currentDate.DayOfWeek; //hente indeks til dagens ukedag

                //"avrunde" dagens dato til nærmeste oppkommende søndag, for å skape et referanse dato
                if (dayOfWeek != 0) //dersom i dag er det søndag (0), gjør ingenting.
                {
                    currentDate = currentDate.AddDays(7 - dayOfWeek); //legg til bestemt antall dager til oppkommende søndag
                }


                //Cruise1
                var week1_mon_hour8 = currentDate.AddDays(1).AddHours(8);

                var week1_mon_hour17 = currentDate.AddDays(1).AddHours(17);

                var week1_wen_hour8 = currentDate.AddDays(3).AddHours(8);

                var week1_wen_hour17 = currentDate.AddDays(3).AddHours(17);

                var week1_fri_hour8 = currentDate.AddDays(5).AddHours(8);

                var week1_fri_hour17 = currentDate.AddDays(5).AddHours(17);

                //Cruise2
                var week1_tue_hour8 = currentDate.AddDays(2).AddHours(8);

                var week1_tue_hour15 = currentDate.AddDays(2).AddHours(15);

                var week1_thu_hour8 = currentDate.AddDays(4).AddHours(8);

                var week1_thu_hour15 = currentDate.AddDays(4).AddHours(15);

                //Cruise3
                var week1_sat_hour10 = currentDate.AddDays(6).AddHours(10);

                //Routes
                var san_str = new Route { Origin = "Sandefjord", Destination = "Strømstad", Return_id = 2 };
                var str_san = new Route { Origin = "Strømstad", Destination = "Sandefjord", Return_id = 1 };
                var stav_berg = new Route { Origin = "Stavanger", Destination = "Bergen", Return_id = 4 };
                var berg_stav = new Route { Origin = "Bergen", Destination = "Stavanger", Return_id = 3 };
                var osl_kie = new Route { Origin = "Oslo", Destination = "Kiel", Return_id = 6 };
                var kie_osl = new Route { Origin = "Kiel", Destination = "Oslo", Return_id = 5 };

                var cruise1_details = new CruiseDetails { Max_Passengers = 10, Passeger_Price = 449, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 99 };
                var cruise2_details = new CruiseDetails { Max_Passengers = 10, Passeger_Price = 549, Passegner_Underage_Price = 399, Pet_Price = 100, Vehicle_Price = 149 };
                var cruise3_details = new CruiseDetails { Max_Passengers = 10, Passeger_Price = 749, Passegner_Underage_Price = 549, Pet_Price = 100, Vehicle_Price = 200 };
                
                //todo: øke antall maks plasser i produksjon, ellers brukes det små tall for å kunne teste.

                var Cruise1 = new Cruise { Route = san_str, CruiseDetails = cruise1_details };
                var Cruise1_rev = new Cruise { Route = str_san, CruiseDetails = cruise1_details };

                var Cruise2 = new Cruise { Route = stav_berg, CruiseDetails = cruise2_details };
                var Cruise2_rev = new Cruise { Route = berg_stav, CruiseDetails = cruise2_details };

                var Cruise3 = new Cruise { Route = osl_kie,  CruiseDetails = cruise3_details };
                var Cruise3_rev = new Cruise { Route = kie_osl, CruiseDetails = cruise3_details };

    
                //Cruise1
                var Schedule1 = new Departure { Cruise = Cruise1, Date = week1_mon_hour8 };
                var Schedule2 = new Departure { Cruise = Cruise1, Date = week1_mon_hour17 };
                var Schedule3 = new Departure { Cruise = Cruise1, Date = week1_wen_hour8 };
                var Schedule4 = new Departure { Cruise = Cruise1, Date = week1_wen_hour17 };
                var Schedule5 = new Departure { Cruise = Cruise1, Date = week1_fri_hour8 };
                var Schedule6 = new Departure { Cruise = Cruise1, Date = week1_fri_hour17 };

                //Cruise1 reverse
                var Schedule7 = new Departure { Cruise = Cruise1_rev, Date = week1_mon_hour8 };
                var Schedule8 = new Departure { Cruise = Cruise1_rev, Date = week1_mon_hour17 };
                var Schedule9 = new Departure { Cruise = Cruise1_rev, Date = week1_wen_hour8 };
                var Schedule10 = new Departure { Cruise = Cruise1_rev, Date = week1_wen_hour17 };
                var Schedule11 = new Departure { Cruise = Cruise1_rev, Date = week1_fri_hour8 };
                var Schedule12 = new Departure { Cruise = Cruise1_rev, Date = week1_fri_hour17 };

                //Cruise2
                var Schedule13 = new Departure { Cruise = Cruise2, Date = week1_tue_hour8 };
                var Schedule14 = new Departure { Cruise = Cruise2, Date = week1_tue_hour15 };
                var Schedule15 = new Departure { Cruise = Cruise2, Date = week1_thu_hour8 };
                var Schedule16 = new Departure { Cruise = Cruise2, Date = week1_thu_hour15 };

                //Cruisea reverse
                var Schedule17 = new Departure { Cruise = Cruise2_rev, Date = week1_tue_hour8 };
                var Schedule18 = new Departure { Cruise = Cruise2_rev, Date = week1_tue_hour15 };
                var Schedule19 = new Departure { Cruise = Cruise2_rev, Date = week1_thu_hour8 };
                var Schedule20 = new Departure { Cruise = Cruise2_rev, Date = week1_thu_hour15 };

                //Cruise3
                var Schedule21 = new Departure { Cruise = Cruise3, Date = week1_sat_hour10 };

                //Cruise3 reverse
                var Schedule22 = new Departure { Cruise = Cruise3_rev, Date = week1_sat_hour10 };


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
