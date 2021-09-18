using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Order_Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Schedule Schedule { get; set; }
        public int Passengers { get; set; }
        public int Passengers_Underage { get; set; }
        public int Pets { get; set; }
        public int Vehicles { get; set; }
    }
}
