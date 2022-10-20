using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Repository.MsSql
{
    public static class SeedData
    {
       


        public static void Seed(IConfiguration configuration)
        {
            var contextOptions = new DbContextOptionsBuilder<ShopContext>()
                .UseSqlServer(configuration.GetConnectionString("MSSQL"))
             .Options;
            using (var dbContext = new ShopContext(contextOptions))
            {
                if (dbContext.Database.GetPendingMigrations().Count() == 0)
                {
                    if (dbContext.Category.Count() == 0)
                    {
                        dbContext.AddRange(Products);

                    }
                    if (dbContext.Products.Count() == 0)
                    {
                        for (int i = 0; i < ProductCategory.Length; i++)
                        {
                            if (dbContext.ProductCetgory.Any(c => c.ProductId == ProductCategory[i].ProductId
                                &&
                                c.CategoryId == ProductCategory[i].CategoryId
                            ))
                            {

                            }
                            else
                            {
                                dbContext.Add<ProductCategory>(ProductCategory[i]);
                            }

                        }
                        dbContext.AddRange(Categories);
                    }
                    dbContext.SaveChanges();
                }
            }
        }
        private static Product[] Products =
        {
            

        };
        private static Category[] Categories =
        {
                
        };
        private static ProductCategory[] ProductCategory =
        {


            };
    }
}


