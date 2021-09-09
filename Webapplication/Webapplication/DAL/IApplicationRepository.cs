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
    }
}
