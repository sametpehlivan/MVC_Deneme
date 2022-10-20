using Microsoft.AspNetCore.Identity;
using Mvc_deneme.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mvc_deneme.CustomValidation
{
    public class CustomUserValidation : IUserValidator<User>
    {
        List<IdentityError> Error = new List<IdentityError>();
       
        
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            string pattern;
            int x;
            pattern = "[a-zA-z0-9(._ğüşöıç)]+";
            Regex rgx = new Regex(@pattern);
            Match result = rgx.Match(user.UserName);
            if (!(result.Value.Length == user.UserName.Length))
            {
                Error.Add(new IdentityError() { Code = "InvalidChar", Description = "._ dışında özel karakter girmeyiniz" });
            }
            if (int.TryParse(user.UserName[0].ToString(),out x))
            {
                Error.Add(new IdentityError() { Code = "StartWithChar", Description = "kullanıcı ismi sayıylabaşlayamaz" });
            }
            if( user.UserName.Length<5 || user.UserName.Length > 20)
            {
                Error.Add(new IdentityError() { Code = "UserNameLength", Description = "kullanıcı adı en az 3 en fazla 20 karakter olmalıdır" });

            }
            if (!Error.Any())
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(Error.ToArray()));
        }
    }
}
