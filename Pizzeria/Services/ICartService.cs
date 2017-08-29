using Pizzeria.Models;
using Pizzeria.Models.OrderViewModels;
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
        bool CartCreated();
        void Customize(OrderCustomizeModel model);
        CartDish GetDish(Guid id);
        List<CartDish> GetAllDishes();
        int OrderTotal();
    }
}
