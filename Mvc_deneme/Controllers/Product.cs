using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.Models;
using System;
using System.Web;
using System.Collections.Generic;
using Mvc_deneme.Extensions;
using System.Threading.Tasks;

namespace Mvc_deneme.Controllers
{
    public class Product : Controller
    {
        public void createMessage(string title, string msg, string alert)
        {
            TempData.Put("message", new AlertMessage()
            {
                AlertType = alert,
                Title = title,
                Message = msg
            });
        }
        private IProductService _productService;
      

        public Product(IProductService productService)
        {
            _productService = productService;
        }

       

        public IActionResult Index(int? page)
        {
            ProductListView model = new ProductListView();
            int size = 2;
            var p = _productService.GetProductsByCategory("", page, size);
            model.Count = (_productService.GetCount("") / size) + (_productService.GetCount("") % size > 0 ? 1:0);
            model.Products = p;
            ViewBag.selectedPage = page ?? 1;
            ViewBag.selectedCategory = "";
            return View(model);
        }
        public IActionResult ProductList(string url,int? page)
        {

            int size = 2;
            if (!string.IsNullOrEmpty(url))
            {
                
                var  model = new ProductListView();
                var p = _productService.GetProductsByCategory(url, page, size);
                model.Count = _productService.GetCount(url) / size + (_productService.GetCount(url) % size > 0 ? 1 : 0);
                model.url = url;
                model.Products = p;
                ViewBag.selectedPage = page ?? 1;
                ViewBag.selectedCategory = url;
                return View(model); 
              
            }
            return RedirectToAction("Index","Product");


        }
        public async Task<IActionResult> Details(int id)
        {
            var p = await _productService.GetId(id);
            if (p == null)
            {
                createMessage("Error","No such product","danger");
            }
            return View(p);
        }       
    }
}
