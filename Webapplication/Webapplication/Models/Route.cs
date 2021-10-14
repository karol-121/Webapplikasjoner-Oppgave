namespace Webapplication.Models
{
    //summary: modell som representerer et rute objekt
    public class Route
    {
        public int Id { get; set; } 
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Return_id { get; set; }
    }
}
