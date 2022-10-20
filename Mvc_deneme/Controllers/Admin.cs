using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.Extensions;
using Mvc_deneme.Identity;
using Mvc_deneme.Models;
using Mvc_deneme.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_deneme.Controllers
{
 
    [Authorize(Roles="admin")]
    public class Admin : Controller
    {
       
        IProductService _productService;
        ICategoryService _categoryService;
        readonly RoleManager<Role> _roleManager;
        readonly UserManager<User> _userManager;
        public Admin(IProductService productService,
                     ICategoryService categoryService,
                     RoleManager<Role> roleManager,
                     UserManager<User> userManager
            )
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Products()
        {        
            var model = await _productService.GetAll();
            return View(model);
        }
        public async  Task<IActionResult> Categories()
        {
            var model= await _categoryService.GetAll();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new EntityLayer.Category()
                {
                    Name = model.Name,
                    Url = NormalizeUrl(model.Name),
                    CreateDateTime= DateTime.Now,
                };
                _categoryService.CreateAsync(entity);
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "KATEGORİ OLUŞTURMA",
                    Message = $"{model.Name} Kategorilere Eklendi"
                }); 

                return RedirectToAction("Categories");
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "success",
                Title = "KATEGORİ OLUŞTURMA",
                Message = $"{model.Name} Kategorilere Eklenemedi Tekrar Deneyiniz"
            });
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int id)
        {
            var entity = await _categoryService.GetId(id);
            if (entity == null) NotFound();
            var model = new CategoryModel()
            {
                Name = entity.Name,
                Id = entity.Id
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryService.GetId(model.Id);
                if (category == null) NotFound();
                category.Id = model.Id;
                category.Name = model.Name;
                category.Url = NormalizeUrl(model.Name);

                _categoryService.Update(category);
                
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "warning",
                    Title = "KATEGORİ GÜNCELLEME",
                    Message = $"{model.Name} Güncellendi"
                });
                return RedirectToAction("Categories");
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "KATEGORİ GÜNCELLEME",
                Message = $"{model.Name} Kategori Güncellenemedi Tekrar Deneyiniz"
            });
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var model = new ProductModel();
            model.ImageUrl = "empty";
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model, int[] categoriesId, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var entity = new EntityLayer.Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Url = NormalizeUrl(model.Name),
                    ImageUrl = model.ImageUrl,
                    Stars = 0,
                    Price = model.Price,
                    UserVote = 0,
                    StockQuantity = model.Quantity,
                    CreateUserName = _userManager.GetUserName(User),
                    CreateDateTime = DateTime.Now
                     
                };
                if (file != null)
                {

                    string fileExt = Path.GetExtension(file.FileName);
                    string rndmName = string.Format($"{Guid.NewGuid()}");
                    entity.ImageUrl = rndmName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\img\\product", rndmName + fileExt);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                }

                if (_productService.Create(entity, categoriesId))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "ÜRÜN OLUŞTURMA",
                        Message = $"{model.Name} Ürünlere Eklendi"
                    });
                   

                    return RedirectToAction("Products");
                }
                   

            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "ÜRÜN OLUŞTURMA",
                Message = $"{model.Name} Ürünlere Eklenemedi Tekrar Deneyiniz"
            });
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }
       
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetId(id);
            if (category == null) return NotFound();
            _categoryService.Delete(category);
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "success",
                Title = "KATEGORİ OLUŞTURMA",
                Message = $"{category.Name} Kategorilerden Silindş"
            });
            return RedirectToAction("Categories");
        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int id)
        {
           
            var p = _productService.GetIdWihtCategory(id);
            var model = new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Url = p.Url,
                ImageUrl = p.ImageUrl,
                rating = p.Stars,
                Price = p.Price,
                Quantity =p.StockQuantity,
                UserVote = p.UserVote,
                SelectedCategory =p.ProductCategories.Select(c=> c.Category).ToList()

            };
            ViewBag.Categories =await _categoryService.GetAll();
            return View(model);
        }

        [HttpPost] 
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoriesId,IFormFile file )
        {
            if (ModelState.IsValid)
            {
                
                var entity = _productService.GetIdWihtCategory(model.Id);
                if (entity == null) return NotFound();
                
                entity.Id = model.Id;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Url = NormalizeUrl(model.Name);
                entity.ImageUrl = model.ImageUrl;
                entity.Stars = model.rating;
                entity.Price = model.Price;
                entity.StockQuantity = model.Quantity;
                entity.UserVote = model.UserVote;

               
                if (file != null)
                {
                    string fileExt = Path.GetExtension(file.FileName);
                    string  rndmName = string.Format($"{Guid.NewGuid()}");
                    entity.ImageUrl = rndmName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\img\\product", rndmName + fileExt);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoriesId))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "PRODUCT GÜNCELLEME",
                        Message = $"{model.Name} Ürün Güncellendi "
                    });
                    return RedirectToAction("Products");
                }
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = "PRODUCT GÜNCELLEME",
                    Message = $"{model.Name} Ürün Güncellenemedi Lütfen Tekrar Deneyiniz."
                });

            }  
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _productService.GetId(id);
           if (product == null) return NotFound();
           _productService.Delete(product);
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "success",
                Title = "PRODUCT SİLME",
                Message = $"{product.Name} Ürün Silindi"
            });
            return  RedirectToAction("Products");
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.roleName.ToLower().Trim());
                if (role !=null)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "danger",
                        Title = "ROL OLUŞTURMA",
                        Message = $"{model.roleName} Role zaten mevcut"
                    });
                    return View(model);
                }
                var result = await _roleManager.CreateAsync(new Role()
                {
                    CreateDate = DateTime.Now,
                    Name = model.roleName
                });
                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "ROL OLUŞTURMA",
                        Message = $"{model.roleName} Role Oluşturuldu"
                    });
                    return RedirectToAction("Roles");
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "ROL OLUŞTURMA",
                Message = $"{model.roleName} Role Oluşturulamadı LütfenTekrar Deneyiniz"
            });
            return View(model);
        }
      
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id) {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var model = new RoleViewModel()
                {
                    roleId = role.Id,
                    roleName = role.Name,
                    roleCreateDate = role.CreateDate
                };
                return View(model);
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "KULLANICI ROL  GÜNCELLEME",
                Message = $"Kullanıcı Bulunamadı  LütfenTekrar Deneyiniz"
            });
            return RedirectToAction("Roles");
        }
       
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.roleId);
                
                if(role == null)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "danger",
                        Title = "ROL GÜNCELLEME",
                        Message = $"{model.roleName} Rol Bulunamadı  LütfenTekrar Deneyiniz"
                    });
                    return View(model);
                }
                string temp = role.Name;
                role.Name = model.roleName;
                var result = _roleManager.UpdateAsync(role);
                if (result.Result.Succeeded)
                {
                    TempData.Put("success", new AlertMessage()
                    {
                        AlertType = "danger",
                        Title = "ROL GÜNCELLEME",
                        Message = $"{temp} Rolü {model.roleName} Olarak Değiştirildi "
                    });
                    return RedirectToAction("Roles");
                }
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = "ROL GÜNCELLEME",
                    Message = $"{temp} Rolü {model.roleName} Olarak Güncellenirken Bir Hata oluştu.LütfenTekrar Deneyiniz"
                });
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if(role != null)
            {
                var result = _roleManager.DeleteAsync(role);
                if (result.Result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "ROL SİLME",
                        Message = $"{role.Name} Rol Silindi"
                       
                    });
                    return RedirectToAction("Roles");
                }
                else 
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "danger",
                        Title = "ROL SİLME",
                        Message = $"{role.Name} Rol Silinirken Bir Hata Oluşty.\nLütfen Tekrar Deneyiniz"
                    });
                }

               
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "ROL SİLME",
                Message = $"{role.Name} Rol Silinemedi.Lütfen Tekrar Deneyiniz"
            });
            return RedirectToAction("Roles");
        }
        
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
        
        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await  _userManager.FindByIdAsync(Id);
            if(user != null)
            {
                var allRole = _roleManager.Roles.ToList();  
                foreach(var item in allRole)
                {
                    Console.WriteLine("{0} allrole",item.Name);
                }
                var  userRole =await _userManager.GetRolesAsync(user);
                
                UserModel model = new UserModel()
                {
                    UserId = Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                    
         
                };
                model.HasRole = new List<HasRoleModel>();
                allRole.ForEach(role => model.HasRole
                    .Add(new HasRoleModel { RoleId = role.Id,
                                            RoleName =role.Name,
                                            hasRole = userRole.Contains(role.Name) 
                                             

                    }));
                return View(model);
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "KULLANICI ROL  GÜNCELLEME",
                Message = $"Kullanıcı Bulunamadı  LütfenTekrar Deneyiniz"
            });
            return RedirectToAction("Users");
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result =  await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    foreach (var role in model.HasRole)
                    {
                        if (role.hasRole)
                            await _userManager.AddToRoleAsync(user, role.RoleName);
                        else
                            await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                    }
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "KULLANICI DÜZENLEME",
                        Message = $"Kullanıcı Bilgileri güncellendi"
                    });
                    return RedirectToAction("Users");
                }
               
                
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "KULLANICI DÜZENLEME",
                Message = $"Bilinmeyen Bir Hata oluştu"
            });

            return RedirectToAction("Users");
        }
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if(user != null)
            {
                var result=await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = "KULLANICI SİLME",
                        Message = $"Kullanıcı  Silindi"
                    });
                    return RedirectToAction("Users");
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "KULLANICI SİLME",
                Message = $"Kullanıcı Bilgileri Silinirken Hata oluştu"
            });
            return RedirectToAction("Users");
        }
        private string NormalizeUrl(string name)
        {
            string url = name;
            url = url.Normalize(NormalizationForm.FormD);
            url = new string(url.Where(c => c < 128).ToArray());
            url = url.ToLower().Trim().Replace(" ", "-");
            return url;

        }

    }
}
