using Microsoft.AspNetCore.Identity;
using Mvc_deneme.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_deneme.CustomValidation
{
    public class CustomPassvordValidation : IPasswordValidator<User>
    {
        List<IdentityError> Error = new List<IdentityError>();
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            if (password.Length <6) {
                Error.Add(new IdentityError() { Code="PasswordLength",Description=" Enter  Minumum 6 character" });
            }
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                Error.Add(new IdentityError() { Code = "BadPassword", Description = "Password don't contain User Name " });
            }
            

            if (!Error.Any()) return Task.FromResult(IdentityResult.Success);
            return  Task.FromResult(IdentityResult.Failed(Error.ToArray()));
        }
    }
}
