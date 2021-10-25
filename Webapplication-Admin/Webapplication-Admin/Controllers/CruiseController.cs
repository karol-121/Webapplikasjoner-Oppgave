﻿using Microsoft.AspNetCore.Http;
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
    public class CruiseController : SharedController
    {
        private readonly string _autorizaionToken = "autorizaionToken";
        private readonly IAppDataRepository _Local_DB; //database objekt

        public CruiseController(IAppDataRepository appDataRepository)
        {
            _Local_DB = appDataRepository;
        }


        //summary: get funksjon for cruise som henter alle cruises som finnes i databasen
        //returns: liste med alle cruise objekter 
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
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
            if (SharedSession.GetString(_autorizaionToken) == "admin")
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
        //parameters: int[] data - [0] id til route objekt som skal til, [1] id til cruise detail objekt som skal til 
        //returns: Http Ok status - ved vellykket lagring, Http Bad request - ved ikke vellykket lagring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPost]
        public async Task<ActionResult> Post(int[] data)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {
                if (await _Local_DB.AddCruise(data[0], data[1])) //data[0] = routeId, data[1] = detailsId
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
        //parameters: int[] - [0] id til cruise objekt som skal endres, [1] id til route objekt som oppdatering, [2] id til cruise details objekt som oppdatering
        //returns: Http Ok status - ved vellykket endring, Http Bad request - ved ikke vellykket endring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPut]
        public async Task<ActionResult> Put(int[] data)
        {
            if (SharedSession.GetString(_autorizaionToken) == "admin")
            {

                if (await _Local_DB.EditCruise(data[0], data[1], data[2])) //data[0] = Id, data[1] = routeId, data[2] = detailsId
                {
                    return Ok("Sucessfullyy changed the cruise");
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
            if (SharedSession.GetString(_autorizaionToken) == "admin")
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
