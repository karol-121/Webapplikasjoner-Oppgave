using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.DAL;
using Webapplication.Models;

namespace Webapplication.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CruiseDetailsController : SharedController
    {
        private readonly string _authorizationToken = "authorizationToken";
        private readonly IAppDataRepository _Local_DB; //database objekt
        private ILogger<CruiseDetailsController> _Local_Log; //log objekt

        public CruiseDetailsController(IAppDataRepository appDataRepository, ILogger<CruiseDetailsController> logger)
        {
            _Local_DB = appDataRepository;
            _Local_Log = logger;
        }

        //summary: get funksjon for cruise details som henter alle cruise details som finnes i databasen
        //returns: liste med alle cruise details objekter 
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                return Ok(await _Local_DB.GetCruisesDetails());
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: get funksjon for cruise details som henter en cruise details med bestemt id 
        //parameters: int id - id til objekt som skal returneres 
        //returns: cruise details
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                return Ok(await _Local_DB.GetCruiseDetails(id));
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: post funksjon for cruise details som lagrer en cruise details objekt
        //parameters: CruiseDetails details - objekt som skal lagres inn i databasen
        //returns: Http Ok status - ved vellykket lagring, Http Bad request - ved ikke vellykket lagring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPost]
        public async Task<ActionResult> Post(CruiseDetails details)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("The new cruise details object cound not be added, invalid input");
                }

                if (await _Local_DB.AddCruiseDetails(details))
                {
                    return Ok("Sucessfully added the new cruise details object");
                }

                return BadRequest("The new cruise details object cound not be added");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: put funksjon for cruise details objekt som endrer en bestemt cruise details objekt
        //parameters: CruiseDetails details - objekt som inneholder informasjon krevet for endring
        //returns: Http Ok status - ved vellykket endring, Http Bad request - ved ikke vellykket endring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPut]
        public async Task<ActionResult> Put(CruiseDetails details)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("The cruise details object could not be changed, invalid input");
                }

                if (await _Local_DB.EditCruiseDetails(details))
                {
                    return Ok("Sucessfullyy changed the cruise details object");
                }

                return BadRequest("The cruise details object could not be changed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: delete funksjon for cruise details objekter som fjerner cruise details objekt med bestemt id
        //parameters: int id - id til den opprinelig objekt som skal fjernes
        //returns: Http Ok status - ved vellykket slettning, Http Bad request - ved ikke vellykket slettning, Http unauthorized - ved uaktorisert tilgang 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (SharedSession.GetString(_authorizationToken) == "admin")
            {
                if (await _Local_DB.DeleteCruiseDetails(id))
                {
                    return Ok("Sucessfully removed the cruise details object");
                }

                return BadRequest("The cruise details object could not be removed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }
    }
}
