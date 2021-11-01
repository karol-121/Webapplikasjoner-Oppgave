
namespace Webapplication.Models
{
    //summary: model på et cruise, cruise er sammen satt av ruten den går på og detalier som pris, max personer osv.
    public class Cruise
    {
        public int Id { get; set; }
        public virtual Route Route { get; set; }
        public virtual CruiseDetails CruiseDetails { get; set; }
    }
}
