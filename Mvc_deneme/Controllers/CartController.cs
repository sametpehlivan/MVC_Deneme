
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.Identity;
using EntityLayer;
using Mvc_deneme.ViewModel;
using BusinessLayer.Abstract;
using System.Collections.Generic;
using System;
using Mvc_deneme.Models;
using Mvc_deneme.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Mvc_deneme.Controllers
{

    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IProductService _productServices;
        private ICartServices _cartService;
        private ICartItemService _cartItemService;
        public CartController(UserManager<User> userManager, 
                              IProductService productService,
                              ICartServices cartServices,
                              ICartItemService cartItemService)
        {
            _userManager = userManager;
            _productServices = productService;
            _cartService = cartServices;
            _cartItemService = cartItemService;
        }
        public IActionResult Buyer()
        {
            return View();
        }
        public async Task<ActionResult> AddCart(int productId,int quantity)
        {
            var product = await _productServices.GetId(productId);

            if (product == null) return NotFound();
           
            
            
            
            var userName = _userManager.GetUserName(User);
            if (userName == null)
            {
                return NotFound();
            }
            var cart = _cartService.GetByUserName(userName);
        
            var cartItem = _cartItemService.GetByCartIdProductId(cart.Id,productId);

            if (cartItem == null) 
            { 
                cartItem = new CartItem() { Quantity = 0, 
                                            ProductId = productId, 
                                            CartId = cart.Id };
               await _cartItemService.CreateAsync(cartItem);
            
            }
            var max = product.StockQuantity > 10 ? 10 : product.StockQuantity;  
           
            if (cartItem.Quantity + quantity <= max && cartItem.Quantity + quantity > 0)
            {
              
                cartItem.Quantity += quantity;
                _cartItemService.Update(cartItem);
                return RedirectToAction("GetCart");
            }
            else if(cartItem.Quantity + quantity > max) 
            {
                TempData.Put("message",new AlertMessage()
                {
                    AlertType = "success",
                    Title = $"Sepete Ekleme",
                    Message = $"{product.Name} Ürün Sepete Ekelendi"
                });
                cartItem.Quantity = max;
                _cartItemService.Update(cartItem);
                return RedirectToAction("GetCart");
            }
            return NotFound();
           
        }

       
        public async Task<IActionResult> GetCart()
        {
            var userName = _userManager.GetUserName(User); 
            var cart = _cartService.GetCartItemByUserName(userName);
            var cartModel = new CartViewModel()
            {
                Id = cart.Id,
                UserName = userName,
              
            };
            
            cartModel.CartItems = new List<CartItemViewModel>();
            foreach (var item in cart.CartItems)
            {
                var product = new EntityLayer.Product();
                product = await _productServices.GetId(item.ProductId);
                if (product.StockQuantity < 1)
                {
                    _cartItemService.Delete(item);
                }
                else
                {
                    cartModel.CartItems.Add(new CartItemViewModel()
                    {
                        Id = item.Id,
                        CartId = item.CartId,
                        Quantity = item.Quantity,
                        Product = product
                    });
                }
                

            }

            return View(cartModel);
        }

        public async Task<IActionResult> DeleteCartItem(int? Id)
        {
            if (Id == null) NotFound();
            var cartItem = await _cartItemService.GetId((int)Id);
            if (cartItem == null) NotFound();
            _cartItemService.Delete(cartItem);
            return RedirectToAction("GetCart");
        }
        public IActionResult DeleteCart(int? Id)
        {
            if (Id == null) NotFound();
            _cartItemService.DeleteByCartId((int)Id);
            return RedirectToAction("GetCart");
        }
    }
}
