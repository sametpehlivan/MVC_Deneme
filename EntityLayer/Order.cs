using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
    public class Order
    {
        
        public int Id { get; set; }
        public string BuyerIdentity { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone {get; set; } 
        public string Email { get; set; }
        public string AddressDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
        public EnumPaymentType PaymentType { get; set; }
        public EnumOrderState OrderState { get; set; }

        public DateTime DateTime { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderProduct> OrderProduct { get; set; }
    }
    public enum EnumOrderState
    {
        waiting = 0,
        unpaid = 1,
        completed = 2
    }
    public enum EnumPaymentType
    {
        creditcard = 0,
        eft = 1,

    }
}

