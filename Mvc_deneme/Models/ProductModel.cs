using EntityLayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.Models
{
    public class ProductModel
    {
        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "İsim alanı boş bırakılamaz")]
        [Display(Name = "Ürün İsmi")]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama alanı boş bırakılamaz")]
        public string Description { get; set; }
       
        public string Url { get; set; }  
        public string ImageUrl { get; set; }
        public double rating { get; set; }
        public int  UserVote { get; set; }
       
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Stock Sayısı")]
        public int Quantity { get; set; }

        
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Fiyat")]
        public double Price { get; set; }
        public List<Category> SelectedCategory {get; set;}
        
    }
}
