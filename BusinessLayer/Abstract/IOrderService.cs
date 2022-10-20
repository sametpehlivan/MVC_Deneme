using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(Order entity);
        void Update(Order entity);
        void Delete(Order entity);
        Task<Order> GetId(int id);
        Task<List<Order>> GetAll();
        bool GetConversationId(string convId);
        int  GetOrderId(string userName, DateTime dateTime);
        List<Order> GetUserName(string username);
        
    }
}
