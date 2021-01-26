using System;
using System.Collections.Generic;

namespace Shop.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
