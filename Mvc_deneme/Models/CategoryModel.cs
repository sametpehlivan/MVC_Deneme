using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.Models
{
    public class CategoryModel
    {
      
        public int Id { get; set; }
        [Required(ErrorMessage = "İsim alanı boş bırakılamaz")]
        [Display(Name = "İsim")]
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
