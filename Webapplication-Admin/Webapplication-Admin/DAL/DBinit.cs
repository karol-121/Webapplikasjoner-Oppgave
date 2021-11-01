using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
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

                //DateTime currentDate = new DateTime(2021, 10, 1, 0, 0, 0); //spesifik dag det skal genereres fra
                DateTime currentDate = DateTime.Today; //nå tid

                int dayOfWeek = (int)currentDate.DayOfWeek; //hente indeks til dagens ukedag

                //"avrunde" dagens dato til nærmeste oppkommende søndag, for å skape et referanse dato
                if (dayOfWeek != 0) //dersom i dag er det søndag (0), gjør ingenting.
                {
                    currentDate = currentDate.AddDays(7 - dayOfWeek); //legg til bestemt antall dager til oppkommende søndag
                }


                //Cruise1
                var week1_mon_hour8 = currentDate.AddDays(1).AddHours(8);
                var week2_mon_hour8 = currentDate.AddDays(8).AddHours(8);

                var week1_mon_hour17 = currentDate.AddDays(1).AddHours(17);
                var week2_mon_hour17 = currentDate.AddDays(8).AddHours(17);

                var week1_wen_hour8 = currentDate.AddDays(3).AddHours(8);
                var week2_wen_hour8 = currentDate.AddDays(10).AddHours(8);

                var week1_wen_hour17 = currentDate.AddDays(3).AddHours(17);
                var week2_wen_hour17 = currentDate.AddDays(10).AddHours(17);

                var week1_fri_hour8 = currentDate.AddDays(5).AddHours(8);
                var week2_fri_hour8 = currentDate.AddDays(12).AddHours(8);

                var week1_fri_hour17 = currentDate.AddDays(5).AddHours(17);
                var week2_fri_hour17 = currentDate.AddDays(12).AddHours(17);

                //Cruise2
                var week1_tue_hour8 = currentDate.AddDays(2).AddHours(8);
                var week2_tue_hour8 = currentDate.AddDays(9).AddHours(8);

                var week1_tue_hour15 = currentDate.AddDays(2).AddHours(15);
                var week2_tue_hour15 = currentDate.AddDays(9).AddHours(15);

                var week1_thu_hour8 = currentDate.AddDays(4).AddHours(8);
                var week2_thu_hour8 = currentDate.AddDays(11).AddHours(8);

                var week1_thu_hour15 = currentDate.AddDays(4).AddHours(15);
                var week2_thu_hour15 = currentDate.AddDays(11).AddHours(15);

                //Cruise3
                var week1_sat_hour10 = currentDate.AddDays(6).AddHours(10);
                var week1_sun_hour10 = currentDate.AddDays(7).AddHours(10);
                var week2_sat_hour10 = currentDate.AddDays(13).AddHours(10);
                var week2_sun_hour10 = currentDate.AddDays(14).AddHours(10);

                //Routes
                var san_str = new Route { Origin = "Sandefjord", Destination = "Strømstad", Return_id = 2 };
                var str_san = new Route { Origin = "Strømstad", Destination = "Sandefjord", Return_id = 1 };
                var stav_berg = new Route { Origin = "Stavanger", Destination = "Bergen", Return_id = 4 };
                var berg_stav = new Route { Origin = "Bergen", Destination = "Stavanger", Return_id = 3 };
                var osl_kie = new Route { Origin = "Oslo", Destination = "Kiel", Return_id = 6 };
                var kie_osl = new Route { Origin = "Kiel", Destination = "Oslo", Return_id = 5 };

                var cruise1_details = new CruiseDetails { Max_Passengers = 500, Passeger_Price = 449, Passegner_Underage_Price = 299, Pet_Price = 100, Vehicle_Price = 99 };
                var cruise2_details = new CruiseDetails { Max_Passengers = 700, Passeger_Price = 549, Passegner_Underage_Price = 399, Pet_Price = 100, Vehicle_Price = 149 };
                var cruise3_details = new CruiseDetails { Max_Passengers = 1000, Passeger_Price = 749, Passegner_Underage_Price = 549, Pet_Price = 100, Vehicle_Price = 200 };

                var Cruise1 = new Cruise { Route = san_str, CruiseDetails = cruise1_details };
                var Cruise1_rev = new Cruise { Route = str_san, CruiseDetails = cruise1_details };

                var Cruise2 = new Cruise { Route = stav_berg, CruiseDetails = cruise2_details };
                var Cruise2_rev = new Cruise { Route = berg_stav, CruiseDetails = cruise2_details };

                var Cruise3 = new Cruise { Route = osl_kie,  CruiseDetails = cruise3_details };
                var Cruise3_rev = new Cruise { Route = kie_osl, CruiseDetails = cruise3_details };

    
                //Cruise1 uke 1
                var Schedule1 = new Departure { Cruise = Cruise1, Date = week1_mon_hour8 };
                var Schedule2 = new Departure { Cruise = Cruise1, Date = week1_mon_hour17 };
                var Schedule3 = new Departure { Cruise = Cruise1, Date = week1_wen_hour8 };
                var Schedule4 = new Departure { Cruise = Cruise1, Date = week1_wen_hour17 };
                var Schedule5 = new Departure { Cruise = Cruise1, Date = week1_fri_hour8 };
                var Schedule6 = new Departure { Cruise = Cruise1, Date = week1_fri_hour17 };

                //Cruise1 uke 2
                var Schedule7 = new Departure { Cruise = Cruise1, Date = week2_mon_hour8 };
                var Schedule8 = new Departure { Cruise = Cruise1, Date = week2_mon_hour17 };
                var Schedule9 = new Departure { Cruise = Cruise1, Date = week2_wen_hour8 };
                var Schedule10 = new Departure { Cruise = Cruise1, Date = week2_wen_hour17 };
                var Schedule11 = new Departure { Cruise = Cruise1, Date = week2_fri_hour8 };
                var Schedule12 = new Departure { Cruise = Cruise1, Date = week2_fri_hour17 };

                //Cruise1 reverse uke 1
                var Schedule13 = new Departure { Cruise = Cruise1_rev, Date = week1_mon_hour8 };
                var Schedule14 = new Departure { Cruise = Cruise1_rev, Date = week1_mon_hour17 };
                var Schedule15 = new Departure { Cruise = Cruise1_rev, Date = week1_wen_hour8 };
                var Schedule16 = new Departure { Cruise = Cruise1_rev, Date = week1_wen_hour17 };
                var Schedule17 = new Departure { Cruise = Cruise1_rev, Date = week1_fri_hour8 };
                var Schedule18 = new Departure { Cruise = Cruise1_rev, Date = week1_fri_hour17 };

                //Cruise1 reverse uke 2
                var Schedule19 = new Departure { Cruise = Cruise1_rev, Date = week2_mon_hour8 };
                var Schedule20 = new Departure { Cruise = Cruise1_rev, Date = week2_mon_hour17 };
                var Schedule21 = new Departure { Cruise = Cruise1_rev, Date = week2_wen_hour8 };
                var Schedule22 = new Departure { Cruise = Cruise1_rev, Date = week2_wen_hour17 };
                var Schedule23 = new Departure { Cruise = Cruise1_rev, Date = week2_fri_hour8 };
                var Schedule24 = new Departure { Cruise = Cruise1_rev, Date = week2_fri_hour17 };

                //Cruise2 uke 1
                var Schedule25 = new Departure { Cruise = Cruise2, Date = week1_tue_hour8 };
                var Schedule26 = new Departure { Cruise = Cruise2, Date = week1_tue_hour15 };
                var Schedule27 = new Departure { Cruise = Cruise2, Date = week1_thu_hour8 };
                var Schedule28 = new Departure { Cruise = Cruise2, Date = week1_thu_hour15 };

                //Cruise2 uke 2
                var Schedule29 = new Departure { Cruise = Cruise2, Date = week2_tue_hour8 };
                var Schedule30 = new Departure { Cruise = Cruise2, Date = week2_tue_hour15 };
                var Schedule31 = new Departure { Cruise = Cruise2, Date = week2_thu_hour8 };
                var Schedule32 = new Departure { Cruise = Cruise2, Date = week2_thu_hour15 };

                //Cruise2 reverse uke 1
                var Schedule33 = new Departure { Cruise = Cruise2_rev, Date = week1_tue_hour8 };
                var Schedule34 = new Departure { Cruise = Cruise2_rev, Date = week1_tue_hour15 };
                var Schedule35 = new Departure { Cruise = Cruise2_rev, Date = week1_thu_hour8 };
                var Schedule36 = new Departure { Cruise = Cruise2_rev, Date = week1_thu_hour15 };

                //Cruise2 reverse uke 2
                var Schedule37 = new Departure { Cruise = Cruise2_rev, Date = week2_tue_hour8 };
                var Schedule38 = new Departure { Cruise = Cruise2_rev, Date = week2_tue_hour15 };
                var Schedule39 = new Departure { Cruise = Cruise2_rev, Date = week2_thu_hour8 };
                var Schedule40 = new Departure { Cruise = Cruise2_rev, Date = week2_thu_hour15 };

                //Cruise3 (reverse) uke 1
                var Schedule41 = new Departure { Cruise = Cruise3, Date = week1_sat_hour10 };
                var Schedule42 = new Departure { Cruise = Cruise3_rev, Date = week1_sun_hour10 };

                //Cruise3 (reverse) uke 2
                var Schedule43 = new Departure { Cruise = Cruise3, Date = week2_sat_hour10 };
                var Schedule44 = new Departure { Cruise = Cruise3_rev, Date = week2_sun_hour10 };

                //departures som skal inn i databasen

                //cruise 1 uke 1
                Context.Departures.Add(Schedule1);
                Context.Departures.Add(Schedule2);
                Context.Departures.Add(Schedule3);
                Context.Departures.Add(Schedule4);
                Context.Departures.Add(Schedule5);
                Context.Departures.Add(Schedule6);

                //cruise 1 uke 2
                Context.Departures.Add(Schedule7);
                Context.Departures.Add(Schedule8);
                Context.Departures.Add(Schedule9);
                Context.Departures.Add(Schedule10);
                Context.Departures.Add(Schedule11);
                Context.Departures.Add(Schedule12);

                //cruise 1 rev uke 1
                Context.Departures.Add(Schedule13);
                Context.Departures.Add(Schedule14);
                Context.Departures.Add(Schedule15);
                Context.Departures.Add(Schedule16);
                Context.Departures.Add(Schedule17);
                Context.Departures.Add(Schedule18);

                //cruise 1 rev uke 2
                Context.Departures.Add(Schedule19);
                Context.Departures.Add(Schedule20);
                Context.Departures.Add(Schedule21);
                Context.Departures.Add(Schedule22);
                Context.Departures.Add(Schedule23);
                Context.Departures.Add(Schedule24);

                //cruise 2 uke 1
                Context.Departures.Add(Schedule25);
                Context.Departures.Add(Schedule26);
                Context.Departures.Add(Schedule27);
                Context.Departures.Add(Schedule28);

                //cruise 2 uke 2
                Context.Departures.Add(Schedule29);
                Context.Departures.Add(Schedule30);
                Context.Departures.Add(Schedule31);
                Context.Departures.Add(Schedule32);

                //cruise 2 rev uke 1
                Context.Departures.Add(Schedule33);
                Context.Departures.Add(Schedule34);
                Context.Departures.Add(Schedule35);
                Context.Departures.Add(Schedule36);

                //cruise 2 rev uke 2
                Context.Departures.Add(Schedule37);
                Context.Departures.Add(Schedule38);
                Context.Departures.Add(Schedule39);
                Context.Departures.Add(Schedule40);

                //cruise 3 (rev) uke 1
                Context.Departures.Add(Schedule41);
                Context.Departures.Add(Schedule42);

                //cruise 3 (rev) uke 2
                Context.Departures.Add(Schedule43);
                Context.Departures.Add(Schedule44);

                Context.SaveChanges();
            }
        }
    }
}
