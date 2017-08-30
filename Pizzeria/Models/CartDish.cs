using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class CartDish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public List<IngredientViewModel> IncludedIngredients { get; set; }
        public List<IngredientViewModel> ExtraIngredients { get; set; }
        public string CategoryName { get; set; }

        public DateTime Added { get; set; }
    }
}
