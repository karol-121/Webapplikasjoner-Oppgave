using System.ComponentModel.DataAnnotations;

namespace Webapplication.Models
{
    //summary: modell på et ordre element som inneholder informasjon krevet for bestilling
    public class OrderItem
    {
        [RegularExpression(@"^[a-zA-ZÆØÅæøå]{2,25}(( |-)[a-zA-ZÆØÅæøå]{1,25}){0,3}$")]
        public string Name { get; set; }
        
        [RegularExpression(@"^[a-zA-ZÆØÅæøå]{2,25}(( |-)[a-zA-ZÆØÅæøå]{1,25}){0,3}$")]
        public string Surname { get; set; }
        
        [RegularExpression(@"^[0-9]{1,3}$")]
        public int Age { get; set; }
        
        [RegularExpression(@"^([a-zA-ZÆØÅæøå]{2,20}){1}( [a-zA-ZÆØÅæøå]{2,20}){0,4}( [0-9]{0,3}){0,1}[a-zA-Z]{0,1}$")]
        public string Address { get; set; }
        
        [RegularExpression(@"^[0-9]{4}$")]
        public string Zip_Code { get; set; }
        
        [RegularExpression(@"^[a-zA-ZÆØÅæøå]{2,25}(( |-)[a-zA-ZÆØÅæøå]{1,25}){0,3}$")]
        public string City { get; set; }
        
        [RegularExpression(@"^(\+\([0-9]{1,3}\)|\+[0-9]{1,3})?( ?[0-9]{1,4}){2,4}$")]
        public string Phone { get; set; }
        
        [RegularExpression(@"^[a-zA-Z0-9._\-]{2,20}@[a-zA-Z0-9._\-]{2,20}$")]
        public string Email { get; set; }
        
        [RegularExpression(@"^[0-9]{1,4}$")]
        public int Departure_Id { get; set; }
        
        [RegularExpression(@"^[0-9]{1,2}$")]
        public int Passengers { get; set; }
        
        [RegularExpression(@"^[0-9]{1,2}$")]
        public int Passengers_Underage { get; set; }
        
        [RegularExpression(@"^[0-9]{1,2}$")]
        public int Pets { get; set; }
        
        [RegularExpression(@"^[0-9]{1,2}$")]
        public int Vehicles { get; set; }

    }
}
