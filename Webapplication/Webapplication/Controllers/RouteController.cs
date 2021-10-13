using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapplication.Models;

namespace Webapplication.Controllers
{
    [Route("API/[controller]")]
    public class RouteController : SharedController
    {
        
        //summary: teste funksjon for å sjekke om shared session fungerer
        public ActionResult Test()
        {
            string a = SharedSession.GetString("autorizaionToken");

            if (a == "admin")
            {
                return Ok("the session token exsist and equals admin");

            } else
            {
                return Unauthorized("You can not access this endpoint");
            }
        }


        //summary: get funksjon for routes som henter alle routes som finnes i databasen
        //returns: liste med alle route objekter 
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("boi");
        }

        //summary: get funksjon for routes som henter en route med bestemt id 
        //parameters: int id - id til objekt som skal returneres 
        //returns: route objekt
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok("boi " + id);
        }

        //summary: post funksjon for routes som lagrer en route
        //i denne tilfellen så er det 2 objekter som lages og lagres, en tur rute og dens retur rute
        //parameters: Route route - objekt som skal lagres inn i databasen
        //returns: Http Ok status - ved vellykket lagring, Http Bad request - ved ikke vellykket lagring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPost]
        public ActionResult Post(Route route)
        {
            return Ok("");
        }

        //summary: put funksjon for routes som endrer en bestemt route 
        //i denne tilfellen så er det 2 objekter som faktisk endres, tur og retur
        //parameters: Route route - objekt som inneholder informasjon krevet for endring
        //returns: Http Ok status - ved vellykket endring, Http Bad request - ved ikke vellykket endring, Http unauthorized - ved uaktorisert tilgang 
        [HttpPut]
        public ActionResult Put(Route route)
        {
            return Ok("");
        }

        //summary: delete funksjon for routes som fjerner route med bestemt id
        //her er det faktisk 2 objekter som fjenres, tur objekt og dens retur
        //parameters: int id - id til den opprinelig objekt som skal fjernes
        //returns: Http Ok status - ved vellykket slettning, Http Bad request - ved ikke vellykket slettning, Http unauthorized - ved uaktorisert tilgang 
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok("");
        }

    }
}
