using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.DAL;
using Microsoft.Extensions.Logging;
using Webapplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace Webapplication.Controllers
{
    [ApiController]
    [Route("API/[action]")]

    //summary: kontroller som sammler endpoints som har med identifikasjon og authorization å gjøre
    //her finnes det mulighet for admin logg inn og admin logg ut. 
    public class AuthController : SharedController
    {
        private readonly string _authorizationToken = "authorizationToken";

        private readonly IAuthRepository _Local_DB; //database objekt

        private ILogger<AuthController> _Local_Log; //log objekt

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _Local_DB = authRepository;
            _Local_Log = logger;
        }

        //summary: funksjon som etablerer et session verdi etter vellykket logg inn. Denne session verdi brukes i andre kontrollerene for å authorization
        //parameters: UserInfo userInfo - objekt som inneholder logg inn detalier som brukernavn og passord
        //returns: Http ok status - ved vellykket identifikasjon, Http bad request status ved feil identifikasjon
        public async Task<ActionResult> EstabilishAdministratorToken(UserInfo userInfo) //change the parameters to userInfor afterwards
        {

            if (!ModelState.IsValid || userInfo == null) //input validering av bruker logg inn info
            {
                _Local_Log.LogError("requested admin authentication with invalid user information");
                return BadRequest("invalid user information");
            }
            

            if (await _Local_DB.AuthenticateAdministrator(userInfo)) //indetifikasjon 
            {
                SharedSession.SetString(_authorizationToken, "admin"); //settes authorization token ved vellykket indtifikasjon
                _Local_Log.LogInformation("admin authorization token set");
                return Ok("sucessfull logg inn");

            } else
            {
                //returneres feilmeldinger ved feil indetifikasjon 
                _Local_Log.LogError("authentication failed");
                return BadRequest("wrong username or password");
            }

            
        }

        //summary: funksjon som fjerner authorization token ved førespørsel
        //returns: Http ok status 
        public ActionResult DemolishAdministratorToken()
        {
            SharedSession.Remove(_authorizationToken);
            _Local_Log.LogInformation("admin authorization token removed");

            return Ok("sucessfull logged out");
        }






        //this endpoint should be deleted toghether with deeper objects in final version
        //this is because this function allows for registring admins that can be used later to logg in
        //there is no requirement that there should be possibillity for creation of admins accounts
        [ExcludeFromCodeCoverage]
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
