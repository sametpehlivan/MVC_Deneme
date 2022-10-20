using DataAccessLayer.Interfaces;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryCart : GenericRepository<Cart>, ICart
    {
       
        public RepositoryCart(ShopContext dbContext):base(dbContext)
        {
           
        }
        private ShopContext shopContext { get { return dbContext as ShopContext; } }
        public Cart GetByUserName(string userName)
        {
            
            return shopContext.Cart.Where(c => c.UserName == userName).FirstOrDefault();
            
        }

        public Cart GetCartItemByUserName(string userName)
        {
           
            return shopContext.Cart.Where(c => c.UserName == userName).Include(c=>c.CartItems).FirstOrDefault();
            
        }
    }
}
