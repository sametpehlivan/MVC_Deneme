using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.MsSql
{
    public class UnitofWork : IUnitOfWork
    {
        private ShopContext dbContext;

        public UnitofWork(ShopContext dbContext )
        {
            this.dbContext = dbContext;
        }

        private RepositoryCart repositoryCart;
        private RepositoryCartItem repositoryCartItem;
        private RepositoryCategory repositoryCategory;
        private RepositoryOrder repositoryOrder;
        private RepositoryOrderProduct repositoryOrderProduct;
        private RepositoryProduct repositoryProduct;

        public ICart Carts => repositoryCart ?? new RepositoryCart(dbContext);
        public ICartItem CartItems => repositoryCartItem ?? new RepositoryCartItem(dbContext);
        public ICategory Categories =>repositoryCategory ??  new RepositoryCategory(dbContext);
        public IOrder Orders => repositoryOrder ?? new RepositoryOrder(dbContext);
        public IOrderProduct OrderProducts => repositoryOrderProduct ?? new RepositoryOrderProduct(dbContext);
        public IProduct Products => repositoryProduct ?? new RepositoryProduct(dbContext);

        public void Dispose()
        {
           dbContext.Dispose();
        }
        public async Task<int>  SaveAsync()
        {
           return await dbContext.SaveChangesAsync();
           
        }
        public void Save()
        {
             dbContext.SaveChanges();
            
        }
    }
}
