using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IOrderProductService
    {
        Task<OrderProduct> CreateAsync(OrderProduct entity);
        void Update(OrderProduct entity);
        void Delete(OrderProduct entity);
        
        Task<List<OrderProduct>> GetAll();
        Task<OrderProduct> GetId(int id);
        List<OrderProduct> GetByOrderId(int orderId);
    }
}
