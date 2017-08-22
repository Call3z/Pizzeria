using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class DishViewModel
    {
        public int DishId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; }

        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}
