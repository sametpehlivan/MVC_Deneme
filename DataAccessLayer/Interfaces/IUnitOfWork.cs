using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICart Carts { get; }
        ICartItem CartItems { get; }
        ICategory Categories { get; }
        IOrder Orders { get; }
        IOrderProduct OrderProducts { get; }
        IProduct Products { get; }
        Task<int> SaveAsync();

        void Save();
    }
}
