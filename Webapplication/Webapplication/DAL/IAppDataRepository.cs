using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IAppDataRepository
    {
        Task<List<Route>> GetRoutes();

        Task<Route> GetRoute(int id);

        Task<bool> AddRoute(Route route);

        Task<bool> EditRoute(Route route);

        Task<bool> DeleteRoute(int id);

    }
}
