using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

                //her kobles det disse to objekter, antaglighivs de burde nå få sitt id?
                //OBS. dette er her er ikke sikkert at det virker, samtidig er det ikke mulig å sjekker ordenlig
                //kan hende at dette må bearbeides senere.
                new_route.Return_id = new_route_return.Id;
                new_route_return.Return_id = new_route.Id;

                await _DB.SaveChangesAsync(); //lagre id
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
                var b = await _DB.Routes.FindAsync(route.Return_id); //hente relatert objekt

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
                CruiseDetails new_cruiseDetails = new CruiseDetails
                {
                    Max_Passengers = details.Max_Passengers,
                    Passeger_Price = details.Passeger_Price,
                    Passegner_Underage_Price = details.Passegner_Underage_Price,
                    Pet_Price = details.Pet_Price,
                    Vehicle_Price = details.Pet_Price
                };

                _DB.CruiseDetails.Add(new_cruiseDetails);

                await _DB.SaveChangesAsync();
                _Local_Log.LogInformation("Sucessfully added new cruise details to db");
                return true;
            }
            catch (Exception e)
            {
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
                var current = await _DB.CruiseDetails.FindAsync(details.Id);

                current.Max_Passengers = details.Max_Passengers;
                current.Passeger_Price = details.Passeger_Price;
                current.Passegner_Underage_Price = details.Passegner_Underage_Price;
                current.Pet_Price = details.Pet_Price;
                current.Vehicle_Price = details.Vehicle_Price;

                await _DB.SaveChangesAsync();

                _Local_Log.LogInformation("Sucessfully changed cruise details in db");
                return true;
            }
            catch (Exception e)
            {
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
                var target = await _DB.CruiseDetails.FindAsync(id);

                _DB.CruiseDetails.Remove(target); 

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
    }
}
