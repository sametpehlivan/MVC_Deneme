using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
    public class Product
    {
       
        public int Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public double Stars { get; set; }
        public int  UserVote { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateUserName { get; set; }
        public  List<ProductCategory> ProductCategories { get; set; }
        
    }
}
