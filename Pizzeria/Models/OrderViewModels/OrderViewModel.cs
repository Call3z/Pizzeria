using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.OrderViewModels
{
    public class OrderViewModel
    {
        public List<CartDish> DishesInCart { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
