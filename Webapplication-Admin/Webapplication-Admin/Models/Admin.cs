
namespace Webapplication.Models
{
    //summary: modell på et admin bruker 
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
