using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Plase enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
