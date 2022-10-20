using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CartItemManager : ICartItemService
    {
        private readonly  IUnitOfWork _unitOfWork;
        public CartItemManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CartItem> CreateAsync(CartItem entity)
        {
           await _unitOfWork.CartItems.CreateAsync(entity);
           await _unitOfWork.SaveAsync();
           return entity;
        }

        public void Delete(CartItem entity)
        {
            _unitOfWork.CartItems.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<List<CartItem>> GetAll()
        {
            return await _unitOfWork.CartItems.GetAll();
        }

        public CartItem GetByCartIdProductId(int cartId, int productId)
        {
            return _unitOfWork.CartItems.GetByCartIdProductId(cartId, productId);
        }

        public async Task<CartItem> GetId(int id)
        {
            return await _unitOfWork.CartItems.GetId(id);
        }

        public void Update(CartItem entity)
        {
            _unitOfWork.CartItems.Update(entity);
            _unitOfWork.Save();
        }
        public void DeleteByCartId(int cartId)
        {
            _unitOfWork.CartItems.DeleteByCartId(cartId);
            _unitOfWork.Save();
        }

        public List<CartItem> GetItemsByCartId(int cartId)
        {
            return _unitOfWork.CartItems.GetItemsByCartId(cartId);
        }

        public double GetTotalPrice(int cartId)
        {
            return _unitOfWork.CartItems.GetTotalPrice(cartId);
        }
    }
}
