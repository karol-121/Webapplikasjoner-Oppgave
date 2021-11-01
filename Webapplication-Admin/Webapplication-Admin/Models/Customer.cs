
namespace Webapplication.Models
{
    //summary: modell på et kunde
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

    }
}
