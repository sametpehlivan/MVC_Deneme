using BusinessLayer.Abstract;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_deneme.EmailSender;
using Mvc_deneme.Extensions;
using Mvc_deneme.Identity;
using Mvc_deneme.Models;
using Mvc_deneme.ViewModel;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Mvc_deneme.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class Account : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IEmailSender _sender;
        private ICartServices _cartService;


        public Account(UserManager<User> userManager,
                       SignInManager<User> signInManager,
                       ICartServices cartService,
                       IEmailSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _sender = sender;

        }
        //
        [HttpGet]
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                
                if(user != null )
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
                  
                    if (result.Succeeded)
                    {

                        await _userManager.ResetAccessFailedCountAsync(user);
                        user.LastLoginDateTime = DateTime.Now;
                        await _userManager.UpdateAsync(user);
                        if (_cartService.GetByUserName(user.UserName) == null)
                        {
                            var cart = new Cart()
                            {
                                UserName = user.UserName,
                            };
                            await _cartService.CreateAsync(cart);
                        }
                       
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "success",
                            Title = $"GİRİŞ : {user.UserName} ",
                            Message = $"{user.FirstName}  {user.LastName} Hoşgeldiniz."
                        });
                        
                        return Redirect("/Home/Index");
                    }
                    await  _userManager.AccessFailedAsync(user);
                    int failedCount = await _userManager.GetAccessFailedCountAsync(user);
                    if ( failedCount >= 5)
                    {
                        ModelState.AddModelError("Lock", "Çok fazla giriş denemesi yapıldı.");
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "danger",
                            Title = $"HESAP KİLİTLENDİ : {user.UserName} ",
                            Message = "Çok fazla giriş denemesi yapıldı."
                        }) ;
                        await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(5));
                    }
                    else
                    {
                        if (result.IsLockedOut) 
                        { 
                            TempData.Put("message", new AlertMessage()
                            {
                                AlertType = "danger",
                                Title = $"HESAP KİLİTLENDİ : {user.UserName} ",
                                Message = "Çok fazla giriş denemesi yapıldı."
                            });
                        
                            ModelState.AddModelError("Lock", "Çok fazla giriş denemesi yapıldı.");
                        }
                        else
                        {
                            TempData.Put("message", new AlertMessage()
                            {
                                AlertType = "danger",
                                Title = $"Bir şeyler ters gitti : {user.UserName} ",
                                Message = "Şifre veya kullanıcı adı hatalı."
                            });
                            ModelState.AddModelError("PasswordError", "E-mail onaylayınız");
                        }

                    }
                    return View();
                }
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = $"KULLANICI MEVCUT DEĞİL : {model.Email} ",
                    Message = "Kullanıcı Bulunamadı."
                });
                ModelState.AddModelError("NotFoundUser","E-mail'e ait kullanıcı bulunmamaktadır");
            }
           
            return View(model);
        }
        
        //
       // [Authorize(Policy ="TimePolicy")]
        [HttpGet]
        public  IActionResult Register()
        {
            return View();
        }
        //[Authorize(Policy = "TimePolicy")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    RegisterDateTime = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {

                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = $"KAYIT",
                        Message = "Kayıt İşlemi Tamamlandı.Lütfen Hesabınızı Onaylayınız"
                    });
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmMail", "Account", new
                    {
                        UserId = user.Id,
                        token = code
                    });

                    //Email
                    await _sender.SendMailAsync(model.Email, "Hesabınızı Onaylıyınız", $"Onaylamak için <a href='https://localhost:44316{url}'>tıklayınız</a>");
                    return RedirectToAction("Login");
                }
                string s = "";
                result.Errors.ToList().ForEach(e => s+=$"{ e.Code}  {e.Description}");
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = $"KAYIT",
                    Message = $"{s}"
                });
            }
         

            return View(model);
        }
        
        //
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "success",
                Title = $"ÇIKIŞ",
                Message = "Görüşmek üzere."
            });
            return Redirect("/login");
        }
            
        
        //
        [HttpGet]
        public IActionResult ResetPassword(string UserId,string token)
        {
            if (UserId == null || token == null) NotFound();
            var model = new ResetPasswordViewModel() { Token = token};
           
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
                    if (result.Succeeded)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "success",
                            Title = $"ŞİFRE",
                            Message = "Şifre Başarıyla Değiştirildi."
                        });

                    }
                    else
                    {
                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "Danger",
                            Title = $"ŞİFRE",
                            Message = "Şifre  Değiştirilemedi."
                        });
                    }
                    
                    return Redirect("/login");
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "Danger",
                Title = $"ŞİFRE",
                Message = "Şifre  Değiştirilemedi."
            });
            return Redirect("/login");
        }
        //
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new
                    {
                        UserId = user.Id,
                        token = token
                    });

                    //Email
                    await _sender.SendMailAsync(user.Email, "Şifre Sıfırlama İsteği", $"Sıfırlamak için <a href='https://localhost:44316{url}'>tıklayınız</a>");
                    
                   
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = $"ŞİFRE",
                        Message = "ŞifreSıfırlama isteği gönderildi."
                    });
                    return Redirect("/login");
                }
              
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = $"ŞİFRE",
                    Message = "Hesap bulunamadı"
                });
                return View();   
            }
            return View();
        }
        //
        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new EditUserViewModel()
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(model);
        }

        [HttpPost]
        public async  Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;  
            user.PhoneNumber = model.PhoneNumber;
            var result = _userManager.UpdateAsync(user);
            if (!result.Result.Succeeded)
            {
                result.Result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = $"HESAP",
                    Message = "Hesap Güncellenemedi"
                });

            }
            else
            {

                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = $"HESAP",
                    Message = "Hesap Güncellendi"
                });
            }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);

            return Redirect("/Home/Index");

        }

        [HttpGet]
        public IActionResult EditPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if(await _userManager.CheckPasswordAsync(user,model.NowPassword))
            {
                var result = _userManager.ChangePasswordAsync(user, model.NowPassword, model.NewPassword);
                if (!result.Result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "Danger",
                        Title = $"ŞİFRE",
                        Message = "Şifre  Değiştirilemedi."
                    });
                    result.Result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    
                }
                else
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType = "success",
                        Title = $"ŞİFRE",
                        Message = "Şifre  Değiştirildi."
                    });
                }
            }

            
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            
            return Redirect("/Home/Index");


        }
        public async Task<IActionResult> ConfirmMail(string userId,string token)
        {
           var user= await _userManager.FindByIdAsync(userId);

           if(user==null) return NotFound();
            var result = _userManager.ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "customer");
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = $"Onay",
                    Message = "Hesap Onaylandı.Giriş Yapabilirsiniz."
                });

            }
            else
            {
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "Danger",
                    Title = $"Onay",
                    Message = "Hesap Onaylanamadı.Giriş Yapabilirsiniz."
                });
            }
            return RedirectToAction("Login");
        }


    }
}
