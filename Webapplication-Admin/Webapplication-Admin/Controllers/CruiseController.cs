﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Webapplication.DAL;
using Webapplication.Models;

namespace Webapplication.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CruiseController : SharedController
    {
        private readonly string _authorizationToken = "authorizationToken";
        private readonly IAppDataRepository _Local_DB; //database objekt
        private ILogger<CruiseController> _Local_Log; //log objekt

        public CruiseController(IAppDataRepository appDataRepository, ILogger<CruiseController> logger)
        {
            _Local_DB = appDataRepository;
            _Local_Log = logger;
        }


        //summary: get funksjon for cruise som henter alle cruises som finnes i databasen
        //returns: liste med alle cruise objekter 
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                return Ok(await _Local_DB.GetCruises());
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: get funksjon for cruises som henter en cruise med bestemt id 
        //parameters: int id - id til objekt som skal returneres 
        //returns: cruise objekt
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                return Ok(await _Local_DB.GetCruise(id));
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: post funksjon for cruises som lagrer en cruise 
        //her passeres det enkle parameterene og ikke objekt dersom dette objektet kun sammensetter andre objekter som allerede eksisterer inn i db
        //for å legge til disse andre objekter, skal man benytte seg av deres add funksjoner og ikke den her.
        //parameters: CruiseBinding cruiseBinding - objekt med data nødvendig for registrering av cruise objekt
        //returns: Http Ok status - ved vellykket lagring, Http Bad request - ved ikke vellykket lagring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPost]
        public async Task<ActionResult> Post(CruiseBinding cruiseBinding)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                if (await _Local_DB.AddCruise(cruiseBinding.routeId, cruiseBinding.detailsId))
                {
                    return Ok("Sucessfully added the new cruise");
                }

                return BadRequest("The new cruise cound not be added");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: put funksjon for cruise som endrer en bestemt cruise
        //her passeres det enkle paremeterene og ikke objekt dersom dette objektet kun sammensetter andre objekter som allerede eksisterer inn i db
        //for å endre disse andre objekter, skal man benytte seg av deres endre funksjoner og ikke den her.
        //parameters: CruiseBinding cruiseBinding - objekt med data nødvendig for endring av cruise objekt
        //returns: Http Ok status - ved vellykket endring, Http Bad request - ved ikke vellykket endring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPut]
        public async Task<ActionResult> Put(CruiseBinding cruiseBinding)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {

                if (await _Local_DB.EditCruise(cruiseBinding.Id, cruiseBinding.routeId, cruiseBinding.detailsId))
                {
                    return Ok("Sucessfully changed the cruise");
                }

                return BadRequest("The cruise could not be changed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: delete funksjon for cruises som fjerner cruise med bestemt id
        //parameters: int id - id til den opprinelig objekt som skal fjernes
        //returns: Http Ok status - ved vellykket slettning, Http Bad request - ved ikke vellykket slettning, Http unauthorized - ved uaktorisert tilgang 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                if (await _Local_DB.DeleteCruise(id))
                {
                    return Ok("Sucessfully removed the cruise");
                }

                return BadRequest("The cruise could not be removed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }


    }
}
