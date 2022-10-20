using System.Collections.Generic;

using EntityLayer;

using Microsoft.AspNetCore.Routing;
namespace Mvc_deneme.Models
{
    public class ProductListView
    {

       public string url { get; set; }
       public int  Count { get; set; }
       
       public List<Product> Products { get; set; }

       
    }
}
