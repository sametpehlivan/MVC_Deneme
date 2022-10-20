using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.ViewModel
{
    public class EditPasswordViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string NowPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        [Compare("NewPassword", ErrorMessage = "Şifre eşleşmiyor.")]
        public string NewConfirmPassword { get; set; }
    }
}
