using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public class AppDataRepository : IAppDataRepository
    {
        private readonly ApplicationDbContext _DB;
        private ILogger<AppDataRepository> _Local_Log;

        public AppDataRepository(ApplicationDbContext DB, ILogger<AppDataRepository> logger)
        {
            _DB = DB;
            _Local_Log = logger;
        }

        //summary: funksjon som henter alle ruter som finnes inn i databasen
        //returns: liste med route objekter
        public async Task<List<Route>> GetRoutes()
        {
            _Local_Log.LogInformation("Requested information about all routes");
            return await _DB.Routes.ToListAsync();
        }

        //summary: funksjon som henter route med bestemt id
        //parameters: int id - id til objektet som skal vises
        //returns: Route objekt
        public async Task<Route> GetRoute(int id)
        {
            _Local_Log.LogInformation("Requested information about route");
            return await _DB.Routes.FindAsync(id);
        }

        //summary: funksjon som legger inn gitt route objekt.
        //her blir det også generet et relatert return objekt som også blir lagt inn
        //parameters: Route route - route objekt som skal legges inn
        public async Task<bool> AddRoute(Route route)
        {
            try
            {
                //lage et ny route objekt som skal inn i databasen
                Route new_route = new Route
                {
                    Origin = route.Origin,
                    Destination = route.Destination,
                };


                //lage et relatert return route 
                Route new_route_return = new Route
                {
                    Origin = route.Destination,
                    Destination = route.Origin,
                };


                //legge begge inn i databasen
                _DB.Routes.Add(new_route);
                _DB.Routes.Add(new_route_return);
                await _DB.SaveChangesAsync(); //man må kalle på save changes for at id blir generated for objekter
                
                //knytte relatert objekter ved hjelp av id som disse objekter skal få inntil nå
                new_route.Return_id = new_route_return.Id;
                new_route_return.Return_id = new_route.Id;
                await _DB.SaveChangesAsync();
                //lagre id
                _Local_Log.LogInformation("Sucessfully added new route/s to db");
                return true;

            }
            catch (Exception e)
            {
                _Local_Log.LogError("error occured while adding new route/s to db:" + e.Message);
                return false;
            }

        }

        //summary: funksjon som endrer allerede eksisterende route objekt med data oppgitt i inn objekt
        //her påføres det også relevante endringer på relatert route objekt 
        //parameters: Route route - route objekt med nye verdier 
        public async Task<bool> EditRoute(Route route)
        {
            try
            {
                var a = await _DB.Routes.FindAsync(route.Id); //hente opprinelig objekt
                var b = await _DB.Routes.FindAsync(a.Return_id); //hente relatert objekt

                //påføre endringer på oppringlig objekt
                a.Origin = route.Origin;
                a.Destination = route.Destination;

                //påføre endringer på relatert objekt
                b.Origin = route.Destination;
                b.Destination = route.Origin;

                //lagre endringer
                await _DB.SaveChangesAsync();

                _Local_Log.LogInformation("Sucessfully changed route/s in db");
                return true;
            }
            catch (Exception e)
            {
                _Local_Log.LogError("Error occured while changing route/s: " + e.Message);
                return false;
            }

        }

        //summary: funksjon som sletter route objekt med bestemt id
        //her blir det også relatert objektet slettet
        //parameters: int id - id til objektet som skal fjernes
        public async Task<bool> DeleteRoute(int id)
        {
            try
            {
                var a = await _DB.Routes.FindAsync(id); //find the requested route to delete
                var b = await _DB.Routes.FindAsync(a.Return_id); //find the related route that also should be deleted

                _DB.Routes.Remove(a); //remove the requested route
                _DB.Routes.Remove(b); //remover related route

                await _DB.SaveChangesAsync(); //lagrer endringer 

                _Local_Log.LogInformation("Sucessfully deleted route/s from db");
                return true;
            }
            catch (Exception e)
            {
                _Local_Log.LogError("error occured while deleting route/s: " + e.Message);
                return false;
            }

        }


        //summary: funksjon som henter alle cruise details objekter som finnes inn i databasen
        //returns: liste med cruise details objekter
        public async Task<List<CruiseDetails>> GetCruisesDetails()
        {
            _Local_Log.LogInformation("Requested all cruises details ");
            return await _DB.CruiseDetails.ToListAsync();
        }

        //summary: funksjon som henter cruise details objekt med bestemt id
        //parameters: int id - id til objektet som skal vises
        //returns: CruiseDetails objekt
        public async Task<CruiseDetails> GetCruiseDetails(int id)
        {
            _Local_Log.LogInformation("Requested information about cruise details");
            return await _DB.CruiseDetails.FindAsync(id);
        }

        //summary: funksjon som legger inn gitt cruise details objekt.
        //parameters: CruiseDetails details - cruise details objekt som skal legges inn
        public async Task<bool> AddCruiseDetails(CruiseDetails details)
        {
            try
            {
                //lage et nytt objekt med data fra inn objekt som skal inn i db
                CruiseDetails new_cruiseDetails = new CruiseDetails
                {
                    Max_Passengers = details.Max_Passengers,
                    Passeger_Price = details.Passeger_Price,
                    Passegner_Underage_Price = details.Passegner_Underage_Price,
                    Pet_Price = details.Pet_Price,
                    Vehicle_Price = details.Pet_Price
                };

                //legg ny objekt inn i databasen og lagre 
                _DB.CruiseDetails.Add(new_cruiseDetails);
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully added new cruise details to db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtering
                _Local_Log.LogError("error occured while adding new cruise details to db: " + e.Message);
                return false;
            }

        }

        //summary: funksjon som endrer allerede eksisterende cruise details objekt med data oppgitt i inn objekt
        //parameters: CruiseDetails details - cruise details objekt med nye verdier 
        public async Task<bool> EditCruiseDetails(CruiseDetails details)
        {
            try
            {
                //hente objektet som skal endres
                var current = await _DB.CruiseDetails.FindAsync(details.Id);

                //oppdatere objektet
                current.Max_Passengers = details.Max_Passengers;
                current.Passeger_Price = details.Passeger_Price;
                current.Passegner_Underage_Price = details.Passegner_Underage_Price;
                current.Pet_Price = details.Pet_Price;
                current.Vehicle_Price = details.Vehicle_Price;

                //lagre endringer
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully changed cruise details in db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtereing
                _Local_Log.LogError("Error occured while changing cruise details in db: " + e.Message);
                return false;
            }

        }

        //summary: funksjon som sletter cruise details objekt med bestemt id
        //parameters: int id - id til objektet som skal fjernes
        public async Task<bool> DeleteCruiseDetails(int id)
        {
            try
            {
                //finne objektet som skal fjernes
                var target = await _DB.CruiseDetails.FindAsync(id);

                //fjerne og lagre
                _DB.CruiseDetails.Remove(target);
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully deleted cruise details from db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtering
                _Local_Log.LogError("error occured while deleting cruise details: " + e.Message);
                return false;
            }
        }


        //summary: funksjon som henter alle cruiser som finnes inn i databasen
        //returns: liste med cruise objekter
        public async Task<List<Cruise>> GetCruises()
        {
            _Local_Log.LogInformation("Requested all cruises");
            return await _DB.Cruises.ToListAsync();
        }

        //summary: funksjon som henter cruise objekt med bestemt id
        //parameters: int id - id til objektet som skal vises
        //returns: Cruise objekt
        public async Task<Cruise> GetCruise(int id)
        {
            _Local_Log.LogInformation("Requested information about cruise");
            return await _DB.Cruises.FindAsync(id);
        }

        //summary: funksjon som skaper og legger inn et nytt cruise objekt ut av parameterene.
        //parameters: int routeId - id til route objekt som skal inn i den nye cruise, int detailsId - id til cruise details objekt som skal inn i den nye cruise 
        public async Task<bool> AddCruise(int routeId, int detailsId)
        {
            try
            {
                //finn objekter som skal være attributer for den nye cruisen
                var route = await _DB.Routes.FindAsync(routeId);
                var details = await _DB.CruiseDetails.FindAsync(detailsId);

                //sjekk om cruise attributer som route og/eller detalier er ikke null, 
                if (route == null || details == null)
                {
                    _Local_Log.LogError("could not add new cruise as route or details or both was not found");
                    return false;
                }

                //lag ny objekt og legg inn attributer 
                Cruise new_cruise = new Cruise
                {
                    Route = route,
                    CruiseDetails = details
                };

                //legg ny objekt inn i databasen og lagre 
                _DB.Cruises.Add(new_cruise);
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully added new cruise to db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtere feil
                _Local_Log.LogError("error occured while adding new cruise to db: " + e.Message);
                return false;
            }

        }

        //summary: funksjon som endrer allerede eksisterende cruise objekt med data oppgitt som parameterene
        //parameters: int Id - id til cruise objekt som skal endres, int routeId - id til route objekt som oppdatering, int detailsId - id til cruise details objekt som oppdatering
        public async Task<bool> EditCruise(int Id, int routeId, int detailsId)
        {
            try
            {
                //finn objekter som skal være attributer for den nye cruisen
                var route = await _DB.Routes.FindAsync(routeId);
                var details = await _DB.CruiseDetails.FindAsync(detailsId);

                //sjekk om cruise attributer som route og/eller detalier er ikke null, 
                if (route == null || details == null)
                {
                    _Local_Log.LogError("could not edit cruise as route or details or both was not found");
                    return false;
                }

                //hente nåværende objekt som skal endres 
                var current = await _DB.Cruises.FindAsync(Id);

                //endre objektet
                current.Route = route;
                current.CruiseDetails = details;

                //lagre endringer
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully changed cruise in db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtere feil
                _Local_Log.LogError("Error occured while changing cruise  in db: " + e.Message);
                return false;
            }
        }

        //summary: funksjon som sletter cruise objekt med bestemt id
        //parameters: int id - id til objektet som skal fjernes
        public async Task<bool> DeleteCruise(int id)
        {
            try
            {
                var target = await _DB.Cruises.FindAsync(id);

                _DB.Cruises.Remove(target);

                await _DB.SaveChangesAsync();

                _Local_Log.LogInformation("Sucessfully deleted cruise details from db");
                return true;
            }
            catch (Exception e)
            {
                _Local_Log.LogError("error occured while deleting cruise details: " + e.Message);
                return false;
            }
        }


        //summary: funksjon som henter alle departures som finnes inn i databasen
        //returns: liste med departure objekter
        public async Task<List<Departure>> GetDepartures()
        {
            _Local_Log.LogInformation("Requested all departures");
            return await _DB.Departures.ToListAsync();
        }

        //summary: funksjon som henter departure objekt med bestemt id
        //parameters: int id - id til objektet som skal vises
        //returns: Departure objekt
        public async Task<Departure> GetDeparture(int id)
        {
            _Local_Log.LogInformation("Requested information about departure");
            return await _DB.Departures.FindAsync(id);
        }

        //summary: funksjon som skaper og legger inn et nytt departure objekt ut av parameterene.
        //parameters: int cruiseId - id til cruise objekt som skal inn i den nye departure, string dateString - string med dato som skal inn i den nye cruise 
        public async Task<bool> AddDeparture(int cruiseId, string dateString)
        {
            try
            {
                //finn objekter som skal være attributer for den nye cruisen
                var cruise = await _DB.Cruises.FindAsync(cruiseId);
                DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                //sjekk om cruise attributer som route og/eller detalier er ikke null, 
                if (cruise == null)
                {
                    _Local_Log.LogError("could not add new departure as cruise was not found");
                    return false;
                }

                //date objekt krever ikke sjekk dersom den vil kaste exception dersom parsing blir ikke vellykket

                //lag ny objekt og legg inn attributer 
                Departure new_departure = new Departure
                {
                    Cruise = cruise,
                    Date = date
                };

                //legg ny objekt inn i databasen og lagre 
                _DB.Departures.Add(new_departure);
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully added new departure to db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtere feil
                _Local_Log.LogError("error occured while adding new departure to db: " + e.Message);
                return false;
            }

        }

        //summary: funksjon som endrer allerede eksisterende departure objekt med data oppgitt som parameterene
        //parameters: int Id - id til departure objekt som skal endres, int cruiseId - id til cruise objekt som oppdatering,
        //string dateString - string med dato som oppdatering
        public async Task<bool> EditDeparture(int Id, int cruiseId, string dateString)
        {
            try
            {
                //finn objekter som skal være attributer for den nye cruisen
                var cruise = await _DB.Cruises.FindAsync(cruiseId);
                DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                //sjekk om cruise attributer som route og/eller detalier er ikke null, 
                if (cruise == null)
                {
                    _Local_Log.LogError("could not edit departure as cruise was not found");
                    return false;
                }

                //date objekt krever ikke sjekk dersom den vil kaste exception dersom parsing blir ikke vellykket

                //hente nåværende objekt som skal endres 
                var current = await _DB.Departures.FindAsync(Id);

                //endre objektet
                current.Cruise = cruise;
                current.Date = date;

                //lagre endringer
                await _DB.SaveChangesAsync();

                //log og return svar
                _Local_Log.LogInformation("Sucessfully changed departure details in db");
                return true;
            }
            catch (Exception e)
            {
                //feil håndtere feil
                _Local_Log.LogError("Error occured while changing departure in db: " + e.Message);
                return false;
            }
        }

        //summary: funksjon som sletter departure objekt med bestemt id
        //parameters: int id - id til objektet som skal fjernes
        public async Task<bool> DeleteDeparture(int id)
        {
            try
            {
                var target = await _DB.Departures.FindAsync(id);

                _DB.Departures.Remove(target);

                await _DB.SaveChangesAsync();

                _Local_Log.LogInformation("Sucessfully deleted departure from db");
                return true;
            }
            catch (Exception e)
            {
                _Local_Log.LogError("error occured while deleting departure: " + e.Message);
                return false;
            }
        }
    }
}
