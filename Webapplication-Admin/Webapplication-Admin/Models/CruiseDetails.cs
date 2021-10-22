using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    //summary: modell som definerer detalier til et cruise
    public class CruiseDetails
    {
        public int Id { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Max_Passengers { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Passeger_Price { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Passegner_Underage_Price { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Pet_Price { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Vehicle_Price { get; set; }
    }
}
