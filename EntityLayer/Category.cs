using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
   public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public  DateTime CreateDateTime { get; set; }
        public  string CreateUserName { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

    }
}
