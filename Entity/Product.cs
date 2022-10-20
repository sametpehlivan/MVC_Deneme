using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public float Stars { get; set; }
        List<Category> Categories { get; set; }
        
    }
}
