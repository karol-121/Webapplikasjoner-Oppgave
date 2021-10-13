using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IAuthRepository
    {
        Task<bool> AuthenticateAdministrator(UserInfo userInfo);

        Task<bool> RegisterAdministrator(UserInfo userInfo); //this should be deleted in final version, more info in auth repo

    }
}
