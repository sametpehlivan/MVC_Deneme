using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface ICartItem : IRepository<CartItem>
    {
        CartItem GetByCartIdProductId(int cartId, int productId);
        List<CartItem> GetItemsByCartId(int cartId);
        void DeleteByCartId(int cartId);
        double GetTotalPrice(int cartId);
    }
}
