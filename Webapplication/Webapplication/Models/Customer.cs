using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public virtual Post Post { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        //Denne metoden sjekker om kunde data er lik, det som er forskjellig fra .equals er at den ekskluderer id parameter
        //todo: sikker denne skal fjernes dersom entity framework liker ikke denne metoden, man kunne kanskje overwrite equals her,
        //men det er sketchy siden equals kan være i bruk i andre ikke opplyste steder
        public bool ContentEquals(Customer other)
        {
            if (other.Name == Name && 
                other.Surname == Surname && 
                other.Age == Age && 
                other.Address == Address && 
                other.Post == Post && 
                other.Phone == Phone && 
                other.Email == Email)
            {
                return true;

            } else
            {
                return false;
            }
        }


    }
}
