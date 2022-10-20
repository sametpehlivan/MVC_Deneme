using System.Collections.Generic;

namespace Mvc_deneme.ViewModel
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
        public double TotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in CartItems)
                totalPrice += item.Quantity * item.Product.Price;
            return totalPrice;
        }
    }

}
