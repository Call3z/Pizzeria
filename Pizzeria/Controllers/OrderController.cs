using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Services;
using Pizzeria.Data;
using Pizzeria.Models.OrderViewModels;
using Pizzeria.ViewModels;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class OrderController : Controller
    {
        private ICartService _cartService;
        private ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context, ICartService cartService)
        {
            _cartService = cartService;
            _context = context;
        }

        public IActionResult Index()
        {
            var dishes = _context.Dishes.Include(di => di.DishIngredients).ThenInclude(i => i.Ingredient).Include(c => c.Category).ToList();
            var viewModel = new OrderViewModel()
            {
                Dishes = dishes,
                DishesInCart = _cartService.CartCreated() ? _cartService.GetAllDishes() : null
            };
            return View(viewModel);
        }

        public IActionResult Add(int id)
        {
            _cartService.AddDish(_context.Dishes.AsNoTracking().Include(di=> di.DishIngredients).ThenInclude(i=> i.Ingredient).Include(c=> c.Category).FirstOrDefault(x => x.DishId.Equals(id)));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            var dish = _cartService.GetDish(id);
            var extras = _context.Ingredients.Where(x=> !dish.IncludedIngredients.Any(y=> y.Id.Equals(x.IngredientId)) && !dish.ExtraIngredients.Any(y=> y.Id.Equals(x.IngredientId))).ToList();
            var extrasViewModel = extras.Select(x => new IngredientViewModel() { Id = x.IngredientId, Name = x.Name, Selected = false, Price = x.Price }).ToList();

            dish.ExtraIngredients.AddRange(extrasViewModel);


            return View(dish);
        }

        [HttpPost]
        public IActionResult Edit(CartDish model)
        {
            _cartService.Customize(model);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(Guid id)
        {
            _cartService.RemoveDish(id);
            return RedirectToAction("Index");
        }
    }
}