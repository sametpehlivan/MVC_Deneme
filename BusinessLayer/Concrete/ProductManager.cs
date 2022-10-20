using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using EntityLayer;
using BusinessLayer.Abstract;


namespace BusinessLayer.Concrete
{
    public  class ProductManager:IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Product> CreateAsync(Product entity)
        {
            await _unitOfWork.Products.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public void Delete(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            _unitOfWork.Save();
        }

       

        public async Task<Product> GetId(int id)
        {
            return await _unitOfWork.Products.GetId(id);
        }

        public List<Product> GetProductsByCategory(string url,int? page,int size)
        {
            if(page==null) page = 1;
            return _unitOfWork.Products.GetProductsByCategory(url,(int)page,size);
        }

        public void  Update(Product entity)
        {
            _unitOfWork.Products.Update(entity);
            _unitOfWork.Save();

        }
        

        public int GetCount(string url)
        {
            return _unitOfWork.Products.GetCount(url);
        }

        public async Task<List<Product>> GetAll()
        {
           return await _unitOfWork.Products.GetAll();
        }

        public Product GetIdWihtCategory(int id)
        {
            return _unitOfWork.Products.GetIdWihtCategory(id);
        }

        public bool Update(Product entity, int[] CategoriesId)
        {

            _unitOfWork.Products.Update(entity, CategoriesId);
            _unitOfWork.Save();
            return true;
        }
        public bool Create(Product entity, int[] categoriesId)
        {
            if (Validation(entity))
            {
                _unitOfWork.Products.Create(entity, categoriesId);
                _unitOfWork.Save();  
            }
            return Validation(entity);
        }
        private bool Validation(Product entity)
        {
           
            if (string.IsNullOrEmpty(entity.Name))
            {
                return false;
            }
            return true;
        }

        public int GetCount()
        {
            return _unitOfWork.Products.GetCount();
        }

        public async Task UpdateAsync(Product entityToUpdate, Product entity)
        {
            entityToUpdate.Name = entity.Name;
            entityToUpdate.Price = entity.Price;
            entityToUpdate.Url = entity.Url;
            entityToUpdate.ImageUrl = entity.ImageUrl;
            entityToUpdate.Description = entity.Description;
            entityToUpdate.Stars = entity.Stars;
            entityToUpdate.UserVote = entity.UserVote;
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            await _unitOfWork.SaveAsync();
            
        }
    }
}
