using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer;
using Mvc_deneme.Models;
using Mvc_deneme.Extensions;
using System.Net.Http;
using Newtonsoft.Json;
namespace Mvc_deneme.Controllers
{
    public class HomeController : Controller
    {
        
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {

             var p = await _productService.GetAll();
            
            

            return View(p);
        }
        public async Task <IActionResult> GetProductsFromRestAPI()
        {
            var products = new List<EntityLayer.Product>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44313/api/products"))
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<EntityLayer.Product>>(responseString);
                   
                }
            }
            return View("Index",products);

        }
        

        
    }
}
