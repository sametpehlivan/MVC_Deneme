using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using DataAccessLayer.Interfaces;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.MsSql
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
       
    {
       protected readonly DbContext dbContext;
        public GenericRepository(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task  CreateAsync(TEntity entity)
        {

            await dbContext.Set<TEntity>().AddAsync(entity);
                     
        }

        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

        public async Task<TEntity> GetId(int id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public async  Task<List<TEntity>> GetAll()
        {

            return  await dbContext.Set<TEntity>().ToListAsync(); 

        }

        public void Update(TEntity entity)
        {
             dbContext.Set<TEntity>().Update(entity);  
            
        }
 
    }
}
