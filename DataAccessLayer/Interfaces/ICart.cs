using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface ICart : IRepository<Cart>
    {
        Cart GetByUserName(string userName);
        Cart  GetCartItemByUserName(string userName);
    }
}
