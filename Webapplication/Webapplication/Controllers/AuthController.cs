using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.DAL;
using Microsoft.Extensions.Logging;
using Webapplication.Models;

namespace Webapplication.Controllers
{
    [Route("API/[action]")]
    public class AuthController : SharedController
    {
        private readonly IAuthRepository _Local_DB; //database objekt

        private ILogger<AuthController> _Local_Log; //log objekt

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _Local_DB = authRepository;
            _Local_Log = logger;
        }


        public async Task<ActionResult> EstabilishAdministratorToken(string u, string p) //change the parameters to userInfor afterwards
        {
            UserInfo userInfo = new UserInfo
            {
                Username = u,
                Password = p
            };

            if (!ModelState.IsValid)
            {
                _Local_Log.LogError("requested admin authentication with invalid user information");
                return BadRequest("invalid user information");
            }
            

            if (await _Local_DB.SetAdministratorSessionToken(userInfo))
            {
                SharedSession.SetString("autorizaionToken", "admin");
                _Local_Log.LogInformation("admin autorization token set");
                return Ok("sucessfull logg inn");

            } else
            {
                _Local_Log.LogError("authentication failed");
                return BadRequest("wrong username or password");
            }

            
        }

        public ActionResult DemolishAdministratorToken()
        {
            SharedSession.Remove("autorizaionToken");
            _Local_Log.LogInformation("admin autorization token removed");

            return Ok("sucessfull logged out");
        }

        //this endpoint should be deleted toghether with deeper objects in final version
        //this is because this function allows for registring admins that can be used later to logg in
        //there is no requirement that there should be possibillity for creation of admins accounts
        public async Task<ActionResult> RegisterUser(string u, string p)
        {
            UserInfo user = new UserInfo
            {
                Username = u,
                Password = p
            };

            await _Local_DB.RegisterAdministrator(user);
            return Ok("admin should be registered");
        }
    }
}
