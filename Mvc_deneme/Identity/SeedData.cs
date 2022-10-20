using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_deneme.Identity
{
    public static class SeedData
    {
        private static string NormalizeName(string name)
        {
            string url = name;
            url = url.Normalize(NormalizationForm.FormD);
            url = new string(url.Where(c => c < 128).ToArray());
            url = url.ToUpper().Trim().Replace(" ", "");
            return url;

        }
        public static async Task  Seed(IConfiguration Conf, UserManager<User> userManager,
                            RoleManager<Role> roleManager)
        {
            var roles = Conf.GetSection("Data:Roles").GetChildren().Select(x => x.Value ).ToArray();
            foreach( var role in roles )
            {
               
                if (await roleManager.FindByNameAsync(role) == null )
                {
                    await roleManager.CreateAsync
                        (
                            new Role()
                            {
                                Name = role,
                                NormalizedName = NormalizeName(role),
                                CreateDate = DateTime.Now
                            }
                        );
                } 
            }
            var users = Conf.GetSection("Data:Users");
            
            foreach( var user in users.GetChildren() )
            {
                
               
                if (await userManager.FindByNameAsync(user.GetValue<string>("UserName")) == null)
                {
                    var userName = user.GetValue<string>("UserName");
                    var email = user.GetValue<string>("Email");
                    var password = user.GetValue<string>("Password");
                    var role = user.GetValue<string>("RoleName");
                    var _user = new User()
                    {
                        Email = email,
                        UserName = userName,
                        NormalizedEmail = NormalizeName(email),
                        NormalizedUserName = NormalizeName(userName),
                        EmailConfirmed = true,
                        RegisterDateTime = DateTime.Now

                    };
                    await userManager.CreateAsync(_user, password);
                    await userManager.AddToRoleAsync(_user, role);
                }
            }
            

        
        }
    }

}

