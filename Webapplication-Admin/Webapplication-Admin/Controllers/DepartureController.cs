using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class DepartureController : SharedController
    {
        private readonly string _autorizaionToken = "autorizaionToken";
        private readonly IAppDataRepository _Local_DB; //database objekt

        public DepartureController (IAppDataRepository appDataRepository)
        {
            _Local_DB = appDataRepository;
        }


        //summary: get funksjon for departure objekter som henter alle utreiser som finnes i databasen
        //returns: liste med alle departure objekter 
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {
                return Ok(await _Local_DB.GetDepartures());
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: get funksjon for departure objekter som henter et utreise med bestemt id 
        //parameters: int id - id til objekt som skal returneres 
        //returns: departure objekt
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {
                return Ok(await _Local_DB.GetDeparture(id));
            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: post funksjon for departures som lagrer en departure objekt
        //her passeres det enkle parameterene og ikke objekt dersom dette objektet kun sammensetter andre objekter og enkelte primitiv datatyper
        //for å legge til disse andre objekter, skal man benytte seg av deres add funksjoner og ikke den her.
        //parameters: int cruiseId - id til cruise objekt som skal til, string dateString - dato streg i format  yyyy-MM-dd som skal til 
        //returns: Http Ok status - ved vellykket lagring, Http Bad request - ved ikke vellykket lagring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPost]
        public async Task<ActionResult> Post(int cruiseId, string dateString)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {
                if (await _Local_DB.AddDeparture(cruiseId, dateString))
                {
                    return Ok("Sucessfully added the new departure");
                }

                return BadRequest("The new departure cound not be added");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: put funksjon for departure objekter som endrer en bestemt utreise
        //her passeres det enkle paremeterene og ikke objekt dersom dette objektet kun sammensetter andre objekter og enkle primitiv datatyper
        //for å endre disse andre objekter, skal man benytte seg av deres endre funksjoner og ikke den her.
        //parameters: int Id - id til departure objekt som skal endres, int cruiseId - id til cruise objekt som oppdatering,
        //string dateString - dato streng i format yyyy-MM-dd som oppdatering
        //returns: Http Ok status - ved vellykket endring, Http Bad request - ved ikke vellykket endring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPut]
        public async Task<ActionResult> Put(int Id, int cruiseId, string dateString)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {

                if (await _Local_DB.EditDeparture(Id, cruiseId, dateString))
                {
                    return Ok("Sucessfullyy changed the departure");
                }

                return BadRequest("The departure could not be changed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

        //summary: delete funksjon for departure objekter som fjerner et utreise med bestemt id
        //parameters: int id - id til den opprinelig objekt som skal fjernes
        //returns: Http Ok status - ved vellykket slettning, Http Bad request - ved ikke vellykket slettning, Http unauthorized - ved uaktorisert tilgang 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {
                if (await _Local_DB.DeleteDeparture(id))
                {
                    return Ok("Sucessfully removed the departure");
                }

                return BadRequest("The departure could not be removed");

            }
            else
            {
                return Unauthorized("Access denied");
            }
        }

    }
}
