﻿using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class CartDish
    {
        public Guid Id { get; set; }
        public int DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public List<IngredientViewModel> Ingredients { get; set; }
        public string CategoryName { get; set; }

        public DateTime Added { get; set; }
    }
}
