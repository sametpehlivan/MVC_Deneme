using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.Models
{
    public class BuyerModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Tam Adres")]
        public string AddressDescription { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "İlçe")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Yalnızca Rakam içermeli")]
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }
      

    }
}
