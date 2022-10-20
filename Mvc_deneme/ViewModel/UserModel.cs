using System.Collections.Generic;

namespace Mvc_deneme.ViewModel
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<HasRoleModel> HasRole { get; set; }
       
    }
}
