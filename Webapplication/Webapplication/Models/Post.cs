using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    //summary: modell på et postnummer og posted representasjon 
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Zip_Code { get; set; }
        public string City { get; set; }
    }
}
