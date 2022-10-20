using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
   public class ProductCategory
    {
        public int ProductId { get; set; }
        List<Product> Product { get; set; }

        public int CategoryId { get; set; }
        List<Category> Category { get; set; }

    }
}
