using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvc_deneme.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private ICategoryService  _categoryService;
        public CategoryViewComponent(ICategoryService categoryService)
        {
           _categoryService = categoryService;
        }
        public async  Task<IViewComponentResult> InvokeAsync()
        {
            
            var entity = await _categoryService.GetAll();
            if(entity == null)
            {
                TempData["message"]="hata";
            }

            List<CategoryModel> model = new List<CategoryModel>();
            foreach (var entityItem in entity)
            {
                model.Add(new CategoryModel
                {
                    Id = entityItem.Id,
                    Name = entityItem.Name,
                    Url = entityItem.Url
                });
            }

            
            return View(model);
        }
    }
}
