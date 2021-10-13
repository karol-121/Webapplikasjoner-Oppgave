using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Controllers
{
    public class SharedController : Controller
    {
        //implements a session object that will be shared across all controllers that implements inherets this controller
        //idea from: https://www.blakepell.com/asp-net-mvc-sharing-a-session-between-controllers
        public ISession SharedSession
        {
            get
            {
                return HttpContext.Session;
            }
        }

    }
}
