using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Controllers
{
    [Route("API/[action]")]
    public class AuthController : SharedController
    {

        
        public ActionResult Estabilish()
        {
            SharedSession.SetString("a", "b");
           
            
            

            return Ok("session token is set");
        }

        public ActionResult Demolish()
        {
            SharedSession.Remove("a");




            return Ok("session token is removed");
        }
    }
}
