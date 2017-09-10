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
                    IncludedIngredients = dish.DishIngredients.Select(x => new IngredientViewModel() { Id = x.Ingredient.IngredientId, Name = x.Ingredient.Name, Price = x.Ingredient.Price, Selected = true }).ToList(),
                    ExtraIngredients = new List<IngredientViewModel>(),
                    Added = DateTime.Now
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
                    IncludedIngredients = dish.DishIngredients.Select(x => new IngredientViewModel() { Id = x.Ingredient.IngredientId, Name = x.Ingredient.Name, Price = x.Ingredient.Price, Selected = true }).ToList(),
                    ExtraIngredients = new List<IngredientViewModel>(),
                    Added = DateTime.Now
                });

                var serialized = JsonConvert.SerializeObject(deserialized);
                _accessor.HttpContext.Session.SetString("Cart", serialized);
            }
        }

        public virtual List<CartDish> GetAllDishes()
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

        public int OrderTotal()
        {
            var deserialized = GetAllDishes();

            int totalPrice = 0;

            foreach (var dish in deserialized)
            {
                var extraSum = dish.ExtraIngredients != null ? dish.ExtraIngredients.Sum(x => x.Price) : 0;

                totalPrice += dish.Price + extraSum;
               
            }

            return totalPrice;
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

        public void Customize(CartDish model)
        {
            var dishes = GetAllDishes();
            var dish = dishes.FirstOrDefault(x => x.Id.Equals(model.Id));

            if(model.IncludedIngredients != null)
            {
                foreach (var ingredient in model.IncludedIngredients)
                {
                    if (ingredient.Selected)
                    {
                        dish.IncludedIngredients.FirstOrDefault(x => x.Id.Equals(ingredient.Id)).Selected = true;
                    }
                    else
                    {
                        dish.IncludedIngredients.FirstOrDefault(x => x.Id.Equals(ingredient.Id)).Selected = false;
                    }
                }
            }

            if(model.ExtraIngredients != null)
            {
                foreach (var ingredient in model.ExtraIngredients)
                {
                    if (ingredient.Selected)
                    {
                        if (!dish.ExtraIngredients.Any(x => x.Id.Equals(ingredient.Id)))
                        {
                            dish.ExtraIngredients.Add(ingredient);
                        }
                        else
                        {
                            dish.ExtraIngredients.FirstOrDefault(x => x.Id.Equals(ingredient.Id)).Selected = true;
                        }
                    }
                    else
                    {
                        if (dish.ExtraIngredients.Any(x => x.Id.Equals(ingredient.Id)))
                        {
                            var ingredientToRemove = dish.ExtraIngredients.FirstOrDefault(x => x.Id.Equals(ingredient.Id));
                            dish.ExtraIngredients.Remove(ingredientToRemove);
                        }
                    }
                }
            }

            var serialized = JsonConvert.SerializeObject(dishes);
            _accessor.HttpContext.Session.SetString("Cart", serialized);
        }

        public bool CartCreated()
        {
            return _accessor.HttpContext.Session.Keys.Any();
        }

        public void RemoveCart()
        {
            _accessor.HttpContext.Session.Clear();
        }
    }
}
