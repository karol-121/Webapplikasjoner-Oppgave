using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class UserInfo
    {
        [RegularExpression(@"[a-zA-Z0-9\-_]{3,15}$")]
        public string Username { get; set; }
        [RegularExpression(@"^[[:ascii:]]{8,64}$")]
        public string Password { get; set; }
    }
}
