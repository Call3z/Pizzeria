using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;
using Newtonsoft.Json;

namespace Pizzeria.Services
{
    public class CartService : ICartService
    {
        private IHttpContextAccessor _accessor;

        public CartService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void AddDish(Dish dish)
        {
            if(_accessor.HttpContext.Session.GetString("Cart") == null)
            {
                var list = new List<CartDish>();

                list.Add(new CartDish() {
                    Id = Guid.NewGuid(),
                    Name = dish.Name,
                    Price = dish.Price,
                    CategoryName = dish.Category.Name,
                    Ingredients = dish.DishIngredients.Select(x=> x.Ingredient.Name).ToList()
                });

                var serialized = JsonConvert.SerializeObject(list);
                _accessor.HttpContext.Session.SetString("Cart", serialized);
            }
            else
            {
                var list = _accessor.HttpContext.Session.GetString("Cart");
                var deserialized = JsonConvert.DeserializeObject<List<CartDish>>(list);

                deserialized.Add(new CartDish() {
                    Id = Guid.NewGuid(),
                    Name = dish.Name,
                    Price = dish.Price,
                    CategoryName = dish.Category.Name,
                    Ingredients = dish.DishIngredients.Select(x => x.Ingredient.Name).ToList()
                });

                var serialized = JsonConvert.SerializeObject(deserialized);
                _accessor.HttpContext.Session.SetString("Cart", serialized);
            }
        }

        public List<CartDish> GetAllDishes()
        {
            var list = _accessor.HttpContext.Session.GetString("Cart");
            return JsonConvert.DeserializeObject<List<CartDish>>(list);
        }

        public CartDish GetDish(Guid id)
        {
            var list = _accessor.HttpContext.Session.GetString("Cart");
            var deserialized = JsonConvert.DeserializeObject<List<CartDish>>(list).SingleOrDefault(x=> x.Id.Equals(id));
            return deserialized;
        }

        public double OrderTotal()
        {
            var list = _accessor.HttpContext.Session.GetString("Cart");
            return JsonConvert.DeserializeObject<List<CartDish>>(list).Sum(x => x.Price);
        }

        public void RemoveDish(Guid id)
        {
            var list = _accessor.HttpContext.Session.GetString("Cart");
            var deserialized = JsonConvert.DeserializeObject<List<CartDish>>(list);
            if(deserialized.Any(x=> x.Id.Equals(id)))
            {
                deserialized.RemoveAll(x => x.Id.Equals(id));
                var serialized = JsonConvert.SerializeObject(deserialized);
                _accessor.HttpContext.Session.SetString("Cart", serialized);
            }

        }
    }
}
