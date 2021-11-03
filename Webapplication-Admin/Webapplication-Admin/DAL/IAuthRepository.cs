using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.DAL
{
    public interface IAuthRepository
    {
        Task<bool> AuthenticateAdministrator(UserInfo userInfo);
        //tilegg funksjon som oppretter admin bruker
        Task<bool> RegisterAdministrator(UserInfo userInfo);
    }
}
