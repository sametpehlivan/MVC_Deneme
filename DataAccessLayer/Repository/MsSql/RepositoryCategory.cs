using System;
using System.Collections.Generic;
using System.Text;
using EntityLayer;
using DataAccessLayer.Interfaces;
namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryCategory : GenericRepository<Category>,ICategory
    {
        public RepositoryCategory(ShopContext dbContext) : base(dbContext)
        {

        }
    }
}
