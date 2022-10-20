using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class OrderManager:IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public async Task<Order> CreateAsync(Order entity)
        {
           await _unitOfWork.Orders.CreateAsync(entity);
           await _unitOfWork.SaveAsync();
           return entity;
        }

        public void Delete(Order entity)
        {
            _unitOfWork.Orders.Delete(entity);
            _unitOfWork.Save();

        }

        public async  Task<List<Order>> GetAll()
        {
            return await _unitOfWork.Orders.GetAll();
        }

        public async Task<Order> GetId(int id)
        {
            return  await _unitOfWork.Orders.GetId(id);
        }

        public void Update(Order entity)
        {
            _unitOfWork.Orders.Update(entity);
            _unitOfWork.Save();
        }
        public bool GetConversationId(string convId)
        {
            return _unitOfWork.Orders.GetConversationId(convId);
        }

        public int GetOrderId(string userName, DateTime dateTime)
        {
            return _unitOfWork.Orders.GetOrderId(userName,dateTime);
        }

        public List<Order> GetUserName(string username)
        {
            return _unitOfWork.Orders.GetUserName(username);
        }
    }
}
