using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryOrderProduct:GenericRepository<OrderProduct>,IOrderProduct
    {
        private ShopContext shopContext { get { return dbContext as ShopContext; } }
        public RepositoryOrderProduct(ShopContext dbContext) : base(dbContext)
        {

        }

        public List<OrderProduct> GetByOrderId(int orderId)
        {
            return shopContext.OrderProduct.Where(op => op.OrderId == orderId).ToList();
            
        }

       
    }
}
