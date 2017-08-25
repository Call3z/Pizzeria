using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Services
{
    public interface ICartService
    {
        void AddDish(Dish dish);
        void RemoveDish(Guid id);
        CartDish GetDish(Guid id);
        List<CartDish> GetAllDishes();
        Double OrderTotal();
    }
}
