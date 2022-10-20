using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
    public class OrderProduct
    {
    
        public int Id { get; set; }
        public  int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }


    }
}
