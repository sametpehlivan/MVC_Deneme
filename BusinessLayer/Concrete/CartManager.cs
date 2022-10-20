using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CartManager : ICartServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartManager(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }
        public async Task<Cart> CreateAsync(Cart entity)
        {
            await _unitOfWork.Carts.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public void Delete(Cart entity)
        {
            _unitOfWork.Carts.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<List<Cart>> GetAll()
        {
            return  await _unitOfWork.Carts.GetAll();
           
        }

        public Cart GetByUserName(string userName)
        {
            return _unitOfWork.Carts.GetByUserName(userName);
        }

        public Cart GetCartItemByUserName(string userName)
        {
            return _unitOfWork.Carts.GetCartItemByUserName(userName);
        }

        public async Task<Cart> GetId(int id)
        {
            return await _unitOfWork.Carts.GetId(id);
        }

        public void Update(Cart entity)
        {
            _unitOfWork.Carts.Update(entity);
            _unitOfWork.Save();
        }
    }
}
