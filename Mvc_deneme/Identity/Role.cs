using Microsoft.AspNetCore.Identity;
using System;

namespace Mvc_deneme.Identity
{
    public class Role : IdentityRole
    {
        public Role()
        {
        }
        public Role(string roleName):base(roleName) {
            CreateDate = DateTime.Now;
        }
        public DateTime CreateDate { get; set; }
    }
}
