namespace Webapplication.Models
{
    public class Cruise
    {
        public int Id { get; set; }
        public virtual Route Route { get; set; }
        public int Departure_Weekday { get; set; }
        public int Departure_Daytime { get; set; }
        public int Max_Passengers { get; set; }
        public int Passeger_Price { get; set; }
        public int Passegner_Underage_Price { get; set; }
        public int Pet_Price { get; set; }
        public int Vehicle_Price { get; set; }
    }
}
