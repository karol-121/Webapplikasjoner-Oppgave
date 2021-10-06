using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class CruiseDetails
    {
        public int Id { get; set; }
        public int Max_Passengers { get; set; }
        public int Passeger_Price { get; set; }
        public int Passegner_Underage_Price { get; set; }
        public int Pet_Price { get; set; }
        public int Vehicle_Price { get; set; }
    }
}
