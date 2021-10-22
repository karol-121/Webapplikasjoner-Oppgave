using System.ComponentModel.DataAnnotations;

namespace Webapplication.Models
{
    //summary: modell som representerer et rute objekt
    public class Route
    {
        public int Id { get; set; }
        [RegularExpression(@"^[A-Za-zøæåØÆÅ\- .]{2,30}$")]
        public string Origin { get; set; }
        [RegularExpression(@"^[A-Za-zøæåØÆÅ\- .]{2,30}$")]
        public string Destination { get; set; }
        public int Return_id { get; set; }
    }
}
