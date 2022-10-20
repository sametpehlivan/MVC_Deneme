using Microsoft.AspNetCore.Identity;

namespace Mvc_deneme.CustomValidation
{
    public class CustomIdentiityErrorDescriber:IdentityErrorDescriber 
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError() { Code = "DublicateUserName", Description = $"{userName} kullanılmaktadır " };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError() { Code = "DublicateEmail", Description = $"{email} kullanılmaktadır " };
        }
        public override IdentityError InvalidUserName(string UserName)
        {
            return new IdentityError() { Code = "InvalidUserName", Description = $"{UserName} kullanılmaktadır " };

        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError() { Code = "InvalidUserName", Description = $"{email} kullanılmaktadır " };

        }
    }
}
