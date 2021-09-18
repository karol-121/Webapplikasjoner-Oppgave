using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class OrderInformation
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Zip_Code { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Departure_Id { get; set; }
        public int Passengers { get; set; }
        public int Passengers_Underage { get; set; }
        public int Pets { get; set; }
        public int Vehicles { get; set; }
    }
}
