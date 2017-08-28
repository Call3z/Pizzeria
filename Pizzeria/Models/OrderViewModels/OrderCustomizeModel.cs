using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.OrderViewModels
{
    public class OrderCustomizeModel
    {
        public CartDish Dish { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; }
    }
}
