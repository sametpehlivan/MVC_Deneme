using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvc_deneme.Identity;
using DataAccessLayer.Repository.MsSql;
namespace Mvc_deneme.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDataBase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using(var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        applicationContext.Database.Migrate();
                    }
                    catch(System.Exception)
                    {
                        throw;
                    }
                }
                using (var shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>())
                {
                    try
                    {
                        shopContext.Database.Migrate();   
                    }
                    catch(System.Exception)
                    {
                        throw;
                    }
                }
                 return host;
            }

        }
    }
}
