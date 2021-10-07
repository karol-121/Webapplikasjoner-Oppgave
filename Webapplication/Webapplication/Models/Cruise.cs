using System;

namespace Webapplication.Models
{
    public class Cruise
    {
        public int Id { get; set; }
        public virtual Route Route { get; set; }
        public virtual CruiseDetails CruiseDetails { get; set; }
    }
}
