using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryCartItem : GenericRepository<CartItem>, ICartItem
    {
        private ShopContext shopContext { get { return (ShopContext)dbContext; } }
        public RepositoryCartItem(ShopContext dbContext) : base(dbContext)
        {

        }
        public CartItem GetByCartIdProductId(int cartId, int productId)
        {
                return shopContext.CartItem.Where(ci=> ci.CartId==cartId && ci.ProductId==productId).FirstOrDefault();
            
        }
        public void DeleteByCartId(int cartId)
        {
           
                var carItems = shopContext.CartItem.Where(ci => ci.CartId == cartId).ToList();
                foreach(var c in carItems)
                {
                    shopContext.CartItem.Remove(c);
                   
                }
            
        }

        public List<CartItem> GetItemsByCartId(int cartId)
        {
            
                return shopContext.CartItem.Where(ci => ci.CartId == cartId).ToList();
                
            
        }

        public double GetTotalPrice(int cartId)
        {
            
                var list = shopContext.CartItem.Where(ci => ci.CartId == cartId).ToList();
                double total = 0;
                foreach(var item in list)
                {
                
                 var product= shopContext.Products.Where(p => p.Id == item.ProductId)
                                                  .Select(p=> p.Price)
                                                  .FirstOrDefault();
                    total += item.Quantity * product;
                }
                return total;
            
        }
    }
}
