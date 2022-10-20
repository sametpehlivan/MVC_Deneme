using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IOrder : IRepository<Order>
    {
        bool GetConversationId(string convId);
        int GetOrderId(string userName, DateTime dateTime);
        List<Order> GetUserName(string username);
    }
}
