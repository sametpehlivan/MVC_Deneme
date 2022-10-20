using System;
using System.Collections.Generic;
using System.Text;
using EntityLayer;

namespace DataAccessLayer.Interfaces
{
    public interface IProduct:IRepository<Product>
    {
        List<Product> GetProductsByCategory(string url, int page, int size);
        int GetCount();
        int GetCount(string url);

        Product GetIdWihtCategory(int id);

        void Update(Product entity, int[] categoriesId);
        void Create(Product entity, int[] categoriesId);

    }
}
