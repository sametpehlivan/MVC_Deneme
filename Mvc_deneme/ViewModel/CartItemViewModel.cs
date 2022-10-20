using EntityLayer;
using Mvc_deneme.Models;

namespace Mvc_deneme.ViewModel
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
       
    }
}
