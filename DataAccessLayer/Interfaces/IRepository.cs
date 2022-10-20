using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> 
        where T : class
    {
        Task CreateAsync(T entity);
        void  Update(T entity);
        void Delete(T entity);
        Task<List<T>> GetAll();
        Task<T> GetId(int id); 
    }
}
