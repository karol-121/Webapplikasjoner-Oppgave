using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Order_Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Cruise Cruise { get; set; }
        public DateTime Cruise_Date { get; set; }
        public bool Return_Trip { get; set; } //bestemmer om kunden har bestillt tur-retur (true) eller tur (false) bilett
        public DateTime Return_Cruise_Date { get; set; } //gjelder kun hvis kunden skal returnere, skal være null dersom biletten er enveis.
        public int Passengers { get; set; }
        public int Passenger_Underage { get; set; }
        public int Pets { get; set; }
        public int Vehicles { get; set; }
    }
}
