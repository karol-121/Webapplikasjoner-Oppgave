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

        Task<List<Cruise>> FindCruises(int RouteId, int Departure_DayOfWeek); 

        Task<List<Cruise>> CheckAvailability(List<Cruise> Cruises, int PassengersAmount, DateTime DepartureDate);

        //Task<Post> FindPost(string Zip_Code);

        //Task RegisterPost(Post post);

        //Task<Customer> FindCustomer(Customer customer);

        //Task RegisterCustomer(Customer customer);

        Task RegisterOrder(OrderInformation OrderInformation);
    }
}
