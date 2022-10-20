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
    public class CategoryManager:ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> CreateAsync(Category entity)
        {
           await _unitOfWork.Categories.CreateAsync(entity);
           await _unitOfWork.SaveAsync();
           return entity;
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<Category> GetId(int id)
        {
            return await _unitOfWork.Categories.GetId(id);
        }

        public async Task<List<Category>> GetAll()
        {

            return await _unitOfWork.Categories.GetAll();
        }

        public void Update(Category entity)
        {
            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Save();
        }
    }
}
