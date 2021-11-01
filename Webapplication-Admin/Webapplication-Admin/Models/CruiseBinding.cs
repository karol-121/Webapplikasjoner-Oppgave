
namespace Webapplication.Models
{
    //objekt som samler enkle datatyper som er nødvendig for modifikasjon av cruise objekt
    public class CruiseBinding
    {
        public int Id { get; set; }
        public int routeId { get; set; }
        public int detailsId { get; set; }
    }
}
