using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IAppDataRepository
    {
        //route objekt
        Task<List<Route>> GetRoutes();
        Task<Route> GetRoute(int id);
        Task<bool> AddRoute(Route route);
        Task<bool> EditRoute(Route route);
        Task<bool> DeleteRoute(int id);

        //cruise details objekt
        Task<List<CruiseDetails>> GetCruisesDetails();
        Task<CruiseDetails> GetCruiseDetails(int id);
        Task<bool> AddCruiseDetails(CruiseDetails details);
        Task<bool> EditCruiseDetails(CruiseDetails details);
        Task<bool> DeleteCruiseDetails(int id);

        //cruise objekt
        Task<List<Cruise>> GetCruises();
        Task<Cruise> GetCruise(int id);
        Task<bool> AddCruise(int routeId, int detailsId);
        Task<bool> EditCruise(int Id, int routeId, int detailsId);
        Task<bool> DeleteCruise(int id);

        //Departure objekt
        Task<List<Departure>> GetDepartures();
        Task<Departure> GetDeparture(int id);
        Task<bool> AddDeparture(int cruiseId, string dateString);
        Task<bool> EditDeparture(int Id, int cruiseId, string dateString);
        Task<bool> DeleteDeparture(int id);

    }
}
