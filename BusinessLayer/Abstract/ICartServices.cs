using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICartServices
    {
        Task<Cart> CreateAsync(Cart entity);
        void Update(Cart entity);
        void Delete(Cart entity);
        Task<List<Cart>> GetAll();
        Task<Cart> GetId(int id);
        Cart GetByUserName(string userName);
        Cart GetCartItemByUserName(string userName);
    }
}
