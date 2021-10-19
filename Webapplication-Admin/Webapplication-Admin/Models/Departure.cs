using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    //summary: model på hvordan et rekord inn i tidstabell skal være, holder informasjon om hva går når
    public class Departure
    {
        public int Id { get; set; }
        public virtual Cruise Cruise { get; set; }
        public DateTime Date { get; set; }
    }
}
