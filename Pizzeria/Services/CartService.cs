using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;
using Newtonsoft.Json;
using Pizzeria.ViewModels;
using Pizzeria.Models.OrderViewModels;

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
                    Ingredients = dish.DishIngredients.Select(x => new IngredientViewModel() { Id = x.Ingredient.IngredientId, Name = x.Ingredient.Name, Selected = true }).ToList()
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
                    Ingredients = dish.DishIngredients.Select(x => new IngredientViewModel() { Id = x.Ingredient.IngredientId, Name = x.Ingredient.Name, Selected = true }).ToList()
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

        public void Customize(OrderCustomizeModel model)
        {
            var dishes = GetAllDishes();
            var dish = dishes.FirstOrDefault(x => x.Id.Equals(model.Dish.Id));

            var notSelectedCurrent = model.Dish.Ingredients?.Where(x => !x.Selected);

            if(notSelectedCurrent != null)
            {
                foreach (var ingredient in notSelectedCurrent)
                {
                    var ingredientToRemove = dish.Ingredients.FirstOrDefault(x => x.Id.Equals(ingredient.Id));
                    if (ingredientToRemove != null)
                    {
                        dish.Ingredients.Remove(ingredientToRemove);
                    }
                }
            }

            var selectedExtras = model.Ingredients?.Where(x => x.Selected);

            if(selectedExtras != null)
            {
                foreach (var extraIngredient in model.Ingredients.Where(x => x.Selected))
                {
                    dish.Ingredients.Add(new IngredientViewModel() { Id = extraIngredient.Id, Name = extraIngredient.Name, Selected = true });
                }
            }

            var serialized = JsonConvert.SerializeObject(dishes);
            _accessor.HttpContext.Session.SetString("Cart", serialized);
        }

        public bool CartCreated()
        {
            return _accessor.HttpContext.Session.Keys.Any();
        }
    }
}
