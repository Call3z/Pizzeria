using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Services;
using Pizzeria.Data;

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
            var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(di => di.Ingredient).Include(x => x.Category).ToList();

            foreach (var item in dish)
            {
                _cartService.AddDish(item);
            }
            return View();
        }

        public IActionResult Test()
        {
            var k = _cartService.GetAllDishes();
            return View("Index");
        }
    }
}