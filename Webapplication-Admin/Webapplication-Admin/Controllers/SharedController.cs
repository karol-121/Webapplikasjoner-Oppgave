using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapplication.Controllers
{
    //summary: kontroller som implementerer et felles session objekt slik at alle kontrollerene som arver den, deler samme session
    //Den brukes for å etablerer et session verdi ved logg inn og sjekke tilgang ved hjelp av den hos andre kontrollerene som skal ha access controll
    public class SharedController : ControllerBase
    {
        //summary: implementasjon av et felles session objekt
        //idea hentet fra: https://www.blakepell.com/asp-net-mvc-sharing-a-session-between-contr
        public ISession SharedSession
        {
            get
            {
                return HttpContext.Session;
            }
        }

    }
}
