using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Controllers
{
    [Route("API/[controller]")]
    public class RouteController : SharedController
    {
        
        public ActionResult Test()
        {
            string a = SharedSession.GetString("a");

            if (!string.IsNullOrEmpty(a))
            {
                return Ok("the session token exsist");
            } else
            {
                return BadRequest("the session token does not exist");
            }
        }

    }
}
