using Mvc_deneme.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc_deneme.ViewModel
{
    public class BuyerViewModel
    {
       public BuyerModel Buyer { get; set; }
       public CartModel Cart { get; set; }
    }
}
