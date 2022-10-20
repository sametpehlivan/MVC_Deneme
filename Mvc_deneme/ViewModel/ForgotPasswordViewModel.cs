using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [EmailAddress]
      
        public string Email { get; set; }
    }
}
