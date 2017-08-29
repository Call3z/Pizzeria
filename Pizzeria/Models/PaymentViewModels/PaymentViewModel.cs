using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.CheckOutViewModels
{
    public class PaymentViewModel
    {
        public List<CartDish> Dishes { get; set; }
        public int OrderTotal { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Cvc { get; set; }
    }
}
