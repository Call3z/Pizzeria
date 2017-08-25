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

        public List<String> Ingredients { get; set; }
        public string CategoryName { get; set; }
    }
}
