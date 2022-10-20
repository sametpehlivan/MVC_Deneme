using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
   public interface ICartItemService
    {
        Task<CartItem> CreateAsync(CartItem entity);
        void Update(CartItem entity);
        void Delete(CartItem entity);
        Task<List<CartItem>> GetAll();
        Task<CartItem> GetId(int id);
        List<CartItem> GetItemsByCartId(int cartId);
        CartItem  GetByCartIdProductId(int cartId, int productId);
        void DeleteByCartId(int cartId);
        double GetTotalPrice(int cartId);
    }
}
