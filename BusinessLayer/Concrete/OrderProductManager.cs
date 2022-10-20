using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class OrderProductManager : IOrderProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public async Task<OrderProduct> CreateAsync(OrderProduct entity)
        {
            await _unitOfWork.OrderProducts.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public void Delete(OrderProduct entity)
        {
            _unitOfWork.OrderProducts.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<List<OrderProduct>> GetAll()
        {
            return await _unitOfWork.OrderProducts.GetAll();   
        }

        public async Task<OrderProduct> GetId(int id)
        {
            return await _unitOfWork.OrderProducts.GetId(id);
        }

        public void Update(OrderProduct entity)
        {
            _unitOfWork.OrderProducts.Update(entity);
            _unitOfWork.Save();
        }
        public List<OrderProduct> GetByOrderId(int orderId)
        {
            return _unitOfWork.OrderProducts.GetByOrderId(orderId);
        }

        
    }
}
