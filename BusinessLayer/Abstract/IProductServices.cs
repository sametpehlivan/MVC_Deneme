using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
namespace BusinessLayer.Abstract
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product entity);    
        Task<List<Product>> GetAll();
        Task<Product> GetId(int id);
        Task UpdateAsync(Product entityToUpdadate, Product entity);
        Task DeleteAsync(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        int GetCount();
        int GetCount(string url);
        List<Product> GetProductsByCategory(string url,int? page,int size);
        Product GetIdWihtCategory(int id);
        bool  Update(Product entity, int[] CategoriesId);
        bool  Create(Product entity, int[] categoriesId);
    }
}
