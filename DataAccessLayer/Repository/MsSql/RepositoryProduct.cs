using System;
using System.Collections.Generic;
using System.Text;
using EntityLayer;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessLayer.Repository.MsSql
{
    public class RepositoryProduct : GenericRepository<Product>, IProduct
    {
        private ShopContext shopContext { get { return dbContext as ShopContext; } }

        public RepositoryProduct(ShopContext dbContext) : base(dbContext)
        {
            
        }
       
        public int GetCount(string url)
        {
                var products = shopContext.Products.AsQueryable();
                if (!string.IsNullOrEmpty(url))
                {
                    products = products.Include(p => p.ProductCategories)
                                      .ThenInclude(pc => pc.Category)
                                      .Where(c => c.ProductCategories
                                      .Any(i => i.Category.Url == url));
                }
                return products.ToList().Count();
        }

        public List<Product> GetProductsByCategory(string url, int page, int size)
        {


                var products = shopContext.Products.AsQueryable();
                if (!string.IsNullOrEmpty(url))
                {
                    products = products.Include(p => p.ProductCategories)
                                      .ThenInclude(pc => pc.Category)
                                      .Where(c => c.ProductCategories
                                      .Any(i => i.Category.Url == url));
                }
                return products.Skip((page - 1) * size).Take(size).ToList();


            

        }

        public Product GetIdWihtCategory(int id)
        {
           return  shopContext.Products.Where(p => id == p.Id)
                                       .Include(p => p.ProductCategories)
                                       .ThenInclude(pc => pc.Category).FirstOrDefault();
        }

        public void Update(Product entity, int[] categoriesId)
        {
            
                var product = shopContext.Products.Include(i => i.ProductCategories).FirstOrDefault(i => i.Id == entity.Id);
                Console.WriteLine(product.Name);
                if (product != null)
                {
                    product.Price = entity.Price;
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.UserVote = entity.UserVote;
                    product.Stars =entity.Stars;
                    product.ImageUrl = entity.ImageUrl;
                    product.StockQuantity = entity.StockQuantity;
                    product.ProductCategories = categoriesId.Select(cat => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = cat,
                    }).ToList();
                   
                }
        }

        public void Create(Product entity, int[] categoriesId)
        {
            
                if (entity != null)
                { 
                    entity.ProductCategories = categoriesId.Select(cat => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = cat,
                    }).ToList();
                    shopContext.Products.Add(entity);
                } 
        }

        public int GetCount()
        {
                return shopContext.Products.Count();   
        }
       
    }
}