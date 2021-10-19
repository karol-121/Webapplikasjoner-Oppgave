using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    //summary: modell på et order som holder et liste med ordre elementer
    public class Order
    {
        public List<OrderItem> Items { get; set; }

    }

    
}
