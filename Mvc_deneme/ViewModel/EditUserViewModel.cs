using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.ViewModel
{
    public class EditUserViewModel
    {
        
        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        [Phone]
        [Display(Name = "Telefon numarası")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }
        [Display(Name = "Soyisim")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public string LastName { get; set; }
    }
}
