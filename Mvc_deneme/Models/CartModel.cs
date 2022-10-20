using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.Models
{
    public class CartModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Kart Numara")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Yalnızca Rakam içermeli")]
        public string CartNumber { get; set; }
        
        
        [MinLength(2)]
        [Display(Name = "İsim")]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "Geçersiz İsim")]
        public string CartFirstName { get; set; }
        
       
        [MinLength(2)]
        [Display(Name = "Soy İsim")]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "Geçersiz Soyisim")]
        public string CartLastName { get; set; }

        [RegularExpression(@"[0-9]+", ErrorMessage = "Yalnızca Rakam içermeli")]
        public string ExpireMonth { get; set; }
        
        [RegularExpression(@"[0-9]+", ErrorMessage = "Yalnızca Rakam içermeli")]
        public string ExpireYear { get; set; }

        [RegularExpression(@"[0-9]+", ErrorMessage = "Yalnızca Rakam içermeli")]
        public string cvc { get; set; }
    }
}
