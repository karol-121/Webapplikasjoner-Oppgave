using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    //summary: modell på hvordan et bilett representeres inn i databasen
    public class Ticket
    {
        public int Id { get; set; }
        public String Session { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Departure Departure { get; set; }
        public int Passengers { get; set; }
        public int Passengers_Underage { get; set; }
        public int Pets { get; set; }
        public int Vehicles { get; set; }
    }
}
