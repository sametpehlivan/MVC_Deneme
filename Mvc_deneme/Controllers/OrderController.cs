using BusinessLayer.Abstract;
using EntityLayer;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.Extensions;
using Mvc_deneme.Identity;
using Mvc_deneme.Models;
using Mvc_deneme.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvc_deneme.Controllers
{
    
    public class OrderController : Controller
    {
        private UserManager<User> _userService;
        private ICartServices _cartService;
        private IOrderProductService _orderProductService;
        private IOrderService _orderService;
        private ICartItemService _cartItemService;
        private IProductService _productService;
        public OrderController(UserManager<User> userService,
                                ICartServices cartService,
                                IOrderProductService orderProductService,
                                IOrderService orderService,
                                ICartItemService cartItemService,
                                IProductService productService)
        {
            _userService = userService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _orderService = orderService;
            _orderProductService = orderProductService;
            _productService = productService;

        }
        [HttpGet]
        public async Task<IActionResult> Buyer()
        {
            var userName = _userService.GetUserName(User);
            var cart = _cartService.GetByUserName(userName);
            var cartItems = _cartItemService.GetItemsByCartId(cart.Id);
            foreach (var item in cartItems)
            {
                var product = await _productService.GetId(item.ProductId);
                if (product != null)
                {
                    if(product.StockQuantity < item.Quantity)
                    {
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "Danger",
                            Title = $"Satın Alma",
                            Message = "Satın Alma Gerçekleştirilemedi.Almak İstediğiniz Ürünün Adedi Stock sayısını aşıyor!!"
                        });
                        return Redirect("/Cart/GetCart");
                    }
                }
                else
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "Danger",
                        Title = $"Satın Alma",
                        Message = "Bir Hata Oluştu Product null"
                    });;
                }
               
            }
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyerAsync(BuyerViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var payment =await  PaymentProcess(model);

                    if (payment.Status == "success")
                    {
                        Console.Write("selam");
                        await SaveOrder(model, payment);
                        _cartItemService.DeleteByCartId(_cartService.GetByUserName(_userService.GetUserName(User)).Id);
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "success",
                            Title = $"Satın Alma",
                            Message = "Satın Alma Gerçekleştirildi"
                        });

                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "danger",
                            Title = $"Satın Alma",
                            Message = "Opss! Beklenmeyen Hata ile karşılaşıldı."
                        });
                        return Redirect("/Home/Index");
                    }
                }catch (Exception ex)
                {
                    
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "danger",
                        Title = $"Hata {ex.Message}",
                        Message = "Opsss!\nBir şeyler Ters gitti lütfen daha sonra tekrar deneyiniz."
                    });
                }
            }
            return View();
        }
        public void SaveOrderProduct(Order model)
        {

        }
        private async Task SaveOrder(BuyerViewModel model,Payment payment)
        {
            var cart = _cartService.GetByUserName(_userService.GetUserName(User));
            var cartItems = _cartItemService.GetItemsByCartId(cart.Id);
  
            double price=0;
            foreach (var item in cartItems)
            {
                item.Product = new EntityLayer.Product();
                item.Product = await _productService.GetId(item.ProductId);
                
                price += item.Quantity * item.Product.Price;
            }
            var dateTime = DateTime.Now;


            var order = new Order()
            {
                UserName = _userService.GetUserName(User),

                FirstName = model.Buyer.FirstName,
                LastName = model.Buyer.LastName,
                Phone = model.Buyer.Phone,
                Email = model.Buyer.Email,
                PaymentId = payment.PaymentId,
                PaymentType = EnumPaymentType.creditcard,
                OrderState = EnumOrderState.waiting,
                ConversationId = payment.ConversationId,
                AddressDescription = model.Buyer.AddressDescription + "\n" + model.Buyer.Town + "/" + model.Buyer.City,
                Country = "Turkey",
                City = model.Buyer.City,
                Town = model.Buyer.Town,
                PostalCode = model.Buyer.ZipCode,
                DateTime = dateTime,
                TotalPrice = price
            };
           await _orderService.CreateAsync(order);
            var orderId=_orderService.GetOrderId(order.UserName,order.DateTime);
            
            foreach(var item in cartItems)
            {
                var product = await _productService.GetId(item.ProductId);
                await _orderProductService.CreateAsync(new OrderProduct()
                {
                    ProductId = item.ProductId,
                    OrderId = orderId,
                    Quantity = item.Quantity,
                    ProductPrice = product.Price,
                    ProductName=product.Name,

                }) ;
                
            }
            foreach(var item in cartItems)
            {
                var product = await _productService.GetId(item.ProductId);
                product.StockQuantity -= item.Quantity;
                _productService.Update(product);
            }
        } 
        private async Task<Payment> PaymentProcess(BuyerViewModel model)
        {
            
            var cart = _cartService.GetByUserName(_userService.GetUserName(User));
            double totalPrice = _cartItemService.GetTotalPrice(cart.Id);
            var convId = new Random().Next(100000000, 999999999);
            var user = await _userService.GetUserAsync(User); 


            Options options = new Options();
            options.ApiKey = "";
            options.SecretKey = "";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId =convId.ToString();
            request.Price = totalPrice.ToString();
            request.PaidPrice = ( totalPrice * 0.2 + totalPrice ).ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = cart.Id.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            
            

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.Cart.CartFirstName +" " + model.Cart.CartLastName;
            paymentCard.CardNumber = model.Cart.CartNumber;
            paymentCard.ExpireMonth = model.Cart.ExpireMonth;
            paymentCard.ExpireYear = model.Cart.ExpireYear;
            paymentCard.Cvc = model.Cart.cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = user.Id;
            buyer.Name = model.Buyer.FirstName;
            buyer.Surname = model.Buyer.LastName;
            buyer.GsmNumber = model.Buyer.Phone;
            buyer.Email = model.Buyer.Email;
            buyer.IdentityNumber = "12345678910"; 
            buyer.LastLoginDate =user.LastLoginDateTime.ToString("yyyy-MM-dd HH:mm:s") ;
            buyer.RegistrationDate = user.RegisterDateTime.ToString("yyyy-MM-dd HH:mm:s");
            Console.WriteLine(buyer.RegistrationDate);
            buyer.RegistrationAddress = model.Buyer.AddressDescription;
            buyer.Ip = "85.34.78.112";
            buyer.City = model.Buyer.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = model.Buyer.ZipCode;
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = model.Buyer.FirstName + " " + model.Buyer.LastName;
            shippingAddress.City = model.Buyer.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = model.Buyer.AddressDescription+"\n"+ model.Buyer.Town + "/" + model.Buyer.City;
            shippingAddress.ZipCode = model.Buyer.ZipCode;
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = model.Buyer.FirstName + " " + model.Buyer.LastName;
            billingAddress.City = model.Buyer.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = model.Buyer.AddressDescription + "\n" + model.Buyer.Town + "/" + model.Buyer.City;
            billingAddress.ZipCode = model.Buyer.ZipCode;
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            var Items = _cartItemService.GetItemsByCartId(cart.Id);
            foreach (var item in Items)
            {
               
                item.Product = new EntityLayer.Product();
                item.Product = _productService.GetIdWihtCategory(item.ProductId);
                var categoryName = item.Product.ProductCategories.Count > 0 ? item.Product.ProductCategories[0].Category.Name : "Kategori yok";
                basketItems.Add(new BasketItem()
                {
                    
                    Id = item.Product.Id.ToString(),
                    Name = item.Product.Name,
                    Category1 = categoryName,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.Product.Price* item.Quantity).ToString(),
            });
               
        }
 
            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
            return payment;

        }
        public async Task<IActionResult> OrderList()
        {
            var model = new List<Order>();    
            model.AddRange(_orderService.GetUserName(_userService.GetUserName(User)));
            foreach(var item in model)
            {
                item.OrderProduct = _orderProductService.GetByOrderId(item.Id);
                foreach (var orderPro in item.OrderProduct)
                {
                    orderPro.Product = new EntityLayer.Product();
                    orderPro.Product = await _productService.GetId(orderPro.ProductId);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> OrderProduct(int orderId)
        {
           
            var model = new Order();
            model = await _orderService.GetId(orderId);
            model.OrderProduct = _orderProductService.GetByOrderId(orderId);
            foreach (var item in model.OrderProduct)
            {
                item.Product = new EntityLayer.Product();
                item.Product = await _productService.GetId(item.ProductId);
            }
            return View(model);
        }
    }
}
