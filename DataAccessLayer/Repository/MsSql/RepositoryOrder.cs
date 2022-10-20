using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryOrder : GenericRepository<Order>, IOrder

    {
        private ShopContext shopContext { get { return dbContext as ShopContext; } }
        public RepositoryOrder(ShopContext dbContext) : base(dbContext)
        {

        }
        public bool  GetConversationId(string convId)
        {
           var orders= shopContext.Order.Where(o => o.ConversationId == convId).ToList();
            if (orders.Count > 0)
            {
                return false;
            }
            return true;             
        }
        public int GetOrderId(string userName, DateTime dateTime)
        {
            var order = shopContext.Order.Where(o => (o.UserName == userName)
                                                    &&
                                                (o.DateTime.Equals(dateTime)))
                                    .FirstOrDefault();
            return order.Id;
        }

        public List<Order> GetUserName(string username)
        {
            return shopContext.Order.Where(o=> (o.UserName == username)).OrderByDescending(o => o.DateTime).ToList();
        }

        
    }
}
