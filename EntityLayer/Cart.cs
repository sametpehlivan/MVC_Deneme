using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> CartItems { get; set;}
    }
}
