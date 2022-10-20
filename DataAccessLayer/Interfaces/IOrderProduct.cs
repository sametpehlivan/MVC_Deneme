using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IOrderProduct : IRepository<OrderProduct>
    {
        List<OrderProduct> GetByOrderId(int orderId);
    }
}
