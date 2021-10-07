using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IApplicationRepository
    {
        Task<List<Route>> GetRoutes();
        Task<List<Departure>> GetDepartures(int Route_Id, DateTime Date_from, DateTime Date_to);
        Task<List<Departure>> CheckAvailability(List<Departure> Departures, int PassengersAmount);
        Task RegisterOrderItem(OrderItem orderItem, string session);
        Task RemoveSessionTickets(string session);
    }
}
