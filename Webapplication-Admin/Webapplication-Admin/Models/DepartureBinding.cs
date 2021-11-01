
namespace Webapplication.Models
{
    //objekt som samler enkle datatyper som er nødvendig for modifikasjon av departure objekt
    public class DepartureBinding
    {
        public int Id { get; set; } 
        public int cruiseId { get; set; }
        public string dateString { get; set; }
    }
}
