using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public virtual Cruise Cruise { get; set; }
        public DateTime Date { get; set; }
    }
}
