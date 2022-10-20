using Microsoft.AspNetCore.Identity;
using System;

namespace Mvc_deneme.Identity
{
    public class User :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public DateTime RegisterDateTime { get; set; }
    }
}
