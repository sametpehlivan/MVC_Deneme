using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Concrete;
using BusinessLayer.Abstract;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository.MsSql;
using Mvc_deneme.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Mvc_deneme.CustomValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using Mvc_deneme.Authorization;
using Microsoft.AspNetCore.Authorization;
using Mvc_deneme.Authorization.Claims;
using Microsoft.AspNetCore.Authentication;
using Mvc_deneme.EmailSender;

namespace Mvc_deneme
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services  )
        {   //
            services.AddDbContext<ApplicationContext>(c => c.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            services.AddDbContext<ShopContext>(c => c.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            services.AddIdentity<User, Role>(options =>
            {
                
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireLowercase = false; 
                options.Password.RequireUppercase = false; 
                options.Password.RequireDigit = false;
               

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefghiýjklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789._";
                options.SignIn.RequireConfirmedEmail = true;

                
            })
            .AddUserValidator<CustomUserValidation>()
            .AddErrorDescriber<CustomIdentiityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
            //
            services.ConfigureApplicationCookie(cookieConf=>
            {
                cookieConf.LoginPath = new PathString("/login");
                cookieConf.Cookie = new CookieBuilder()
                {
                    Name = "Cookie",
                    HttpOnly = false,
                    SecurePolicy = CookieSecurePolicy.Always,
                    SameSite = SameSiteMode.Lax
                };
                cookieConf.SlidingExpiration = true;
                cookieConf.ExpireTimeSpan = TimeSpan.FromDays(1);
                cookieConf.AccessDeniedPath = "/authority/page";
            }
            );
            //
            services.AddAuthorization(builder=>builder.AddPolicy("TimePolicy",
                            policy => policy.Requirements.Add(new TimeRequirement())));
            services.AddSingleton<IAuthorizationHandler,TimeHandler>();

            //
            //services.AddScoped<IClaimsTransformation, UserClaimProvider>();
            //
            services.AddScoped<IUnitOfWork, UnitofWork>();

            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IProductService,ProductManager>();
            services.AddScoped<ICartServices, CartManager>();
            services.AddScoped<ICartItemService,CartItemManager>();
            services.AddScoped<IOrderService,OrderManager >();
            services.AddScoped<IOrderProductService, OrderProductManager>();

            
            //
            //services.AddScoped<IOrderProduct, RepositoryOrderProduct>();
            //services.AddScoped<IOrder, RepositoryOrder>();
            //services.AddScoped<ICartItem, RepositoryCartItem>();
            //services.AddScoped<ICart, RepositoryCart>();
            //services.AddScoped<IProduct, RepositoryProduct>();
            //services.AddScoped<ICategory, RepositoryCategory>();
            //
            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:Username"],
                    Configuration["EmailSender:Password"]
                ));
            services.AddControllersWithViews();
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,                             
                              IConfiguration configuration,UserManager<User> userManager,
                              RoleManager<Role> roleManager)
        {
           
            if (env.IsDevelopment())
            {
              
                app.UseDeveloperExceptionPage();
                
               DataAccessLayer.Repository.MsSql.SeedData.Seed(configuration);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(
                    name: "edituser",
                    pattern: "/cart",
                    defaults: new { controller = "Cart", Action = "GetCart" }
                    );

                endpoints.MapControllerRoute(
                    name: "edituser",
                    pattern: "/editpassword",
                    defaults: new { controller = "Account", Action = "EditPassword" }
                    );
                endpoints.MapControllerRoute(
                    name: "edituser",
                    pattern: "/editprofile",
                    defaults: new { controller = "Account", Action = "EditUser" }
                    );
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/logout",
                    defaults: new { controller = "Account", Action = "Logout" }
                    );
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/login",
                    defaults: new { controller = "Account", Action = "Login" }
                    );
                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "/register",
                    defaults: new { controller = "Account", Action = "Register" }
                    );
                endpoints.MapControllerRoute(
                   name: "users",
                   pattern: "/admin/user",
                   defaults: new { controller = "Admin", Action = "Users" }
                   );
                endpoints.MapControllerRoute(
                   name: "edituser",
                   pattern: "/user/edit/{id?}",
                   defaults: new { controller = "Admin", Action = "EditUser" }
                   );
                endpoints.MapControllerRoute(
                   name: "deleteuser",
                   pattern: "/delete/user/{id?}",
                   defaults: new { controller = "Admin", Action = "DeleteUser" }
                   );
                endpoints.MapControllerRoute(
                   name: "editrole",
                   pattern: "/delete/role/{id?}",
                   defaults: new { controller = "Admin", Action = "DeleteRole" }
                   );
                endpoints.MapControllerRoute(
                   name: "editrole",
                   pattern: "/role/edit/{id?}",
                   defaults: new { controller = "Admin", Action = "EditRole" }
                   );
                endpoints.MapControllerRoute(
                   name: "createrole",
                   pattern: "/create/role",
                   defaults: new { controller = "Admin", Action = "CreateRole" }
                   );
                endpoints.MapControllerRoute(
                   name: "listroles",
                   pattern: "/admin/role",
                   defaults: new { controller = "Admin", Action = "Roles" }
                   );
                endpoints.MapControllerRoute(
                    name: "admincategorylist",
                    pattern: "/delete/category/{id?}",
                    defaults: new { controller = "Admin", Action = "DeleteCategory" }
                    );
                endpoints.MapControllerRoute(
                    name: "admincategorylist",
                    pattern: "/create/category",
                    defaults: new { controller = "Admin", Action = "CreateCategory" }
                    );
                endpoints.MapControllerRoute(
                    name: "admincategorylist",
                    pattern: "/admin/category",
                    defaults: new { controller = "Admin", Action = "Categories" }
                    );
                endpoints.MapControllerRoute(
                    name: "admincategoryedit",
                    pattern: "/category/edit/{id?}",
                    defaults: new { controller = "Admin", Action = "CategoryEdit" }
                    );
                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "/product/edit/{id?}",
                    defaults: new { controller = "Admin", Action = "ProductEdit" }
                    );
                endpoints.MapControllerRoute(
                    name: "adminedit",
                    pattern: "/delete/product/{id?}",
                    defaults: new { controller = "Admin", Action = "ProductDelete" }
                    );
                endpoints.MapControllerRoute(
                    name:"adminlist",
                    pattern :"/admin/product",
                    defaults:new { controller ="Admin",Action="Products"}
                    );
                endpoints.MapControllerRoute(
                    name: "createproduct",
                    pattern: "/create/product",
                    defaults: new { controller = "Admin", Action = "CreateProduct" }
                    );

                endpoints.MapControllerRoute(
                    name:"details",
                    pattern: "/details/{id?}",
                    defaults : new {controller = "Product",Action="Details"}
                    );
                endpoints.MapControllerRoute(
                  name: "category",
                  pattern: "/product/{url}/{page?}",
                  defaults: new { controller = "Product", Action = "ProductList" }
                  );
                endpoints.MapControllerRoute(
                    name: "index",
                    pattern: "/product/index/{page?}",
                    defaults: new { controller = "Product", Action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
                Identity.SeedData.Seed(configuration, userManager, roleManager).Wait();
            
            

        }
    }
}
