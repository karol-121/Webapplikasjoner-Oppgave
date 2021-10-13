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
            string a = SharedSession.GetString("autorizaionToken");

            if (a == "admin")
            {
                return Ok("the session token exsist and equals admin");

            } else
            {
                return Unauthorized("You can not access this endpoint");
            }
        }

    }
}
