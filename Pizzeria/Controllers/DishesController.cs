using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.ViewModels;

namespace Pizzeria.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(k=> k.DishIngredients)
                .ThenInclude(di=> di.Ingredient)
                .Include(c=> c.Category)
                .SingleOrDefaultAsync(m => m.DishId == id);


            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var ingredients = _context.Ingredients.ToList();

            var ingredientViewModel = new List<IngredientViewModel>();
            foreach(var ingredient in ingredients)
            {
                ingredientViewModel.Add(new IngredientViewModel() { Id = ingredient.IngredientId, Name = ingredient.Name, Selected = false });
            }

            var viewModel = new DishViewModel() { Categories = categories, Ingredients = ingredientViewModel };
            return View(viewModel);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishViewModel model)
        {
            if (ModelState.IsValid)
            {

                var dishIngredients = new List<DishIngredient>();
                foreach (var ingredient in model.Ingredients.Where(x=> x.Selected))
                {
                    dishIngredients.Add(new DishIngredient() { IngredientId = ingredient.Id });
                }

                var category = _context.Categories.FirstOrDefault(x => x.CategoryId.Equals(model.CategoryId));

                var dish = new Dish()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Category = category,
                    DishIngredients = dishIngredients
                };

                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.
                Include(x=> x.Category).
                Include(x=> x.DishIngredients).
                ThenInclude(x=> x.Ingredient).
                SingleOrDefaultAsync(m => m.DishId == id);

            var allCategories = await _context.Categories.ToListAsync();
            var allIngredients = await _context.Ingredients.Select(x=> new IngredientViewModel()
            {
                Id = x.IngredientId,
                Name = x.Name,
                Selected = dish.DishIngredients.Any(k=> k.IngredientId.Equals(x.IngredientId) ? true : false)
            }).ToListAsync();

            var viewModel = new DishViewModel()
            {
                DishId = dish.DishId,
                Name = dish.Name,
                Price = dish.Price,
                CategoryId = dish.Category.CategoryId,
                Ingredients = allIngredients,
                Categories = allCategories
            };

            if (dish == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("DishId,Name,Price,Ingredients")] Dish dish
        public async Task<IActionResult> Edit(DishViewModel model)
        {

            if (ModelState.IsValid)
            {
                var dish = _context.Dishes.Include(x=> x.DishIngredients).FirstOrDefault(x => x.DishId.Equals(model.DishId));
                dish.CategoryId = model.CategoryId;
                dish.Name = model.Name;
                dish.Price = model.Price;

                foreach (var ingredient in model.Ingredients)
                {
                    if (ingredient.Selected && !dish.DishIngredients.Any(x=> x.IngredientId.Equals(ingredient.Id)))
                    {
                        dish.DishIngredients.Add(new DishIngredient() { IngredientId = ingredient.Id });
                    }
                    else if(!ingredient.Selected && dish.DishIngredients.Any(x=> x.IngredientId.Equals(ingredient.Id)))
                    {
                        dish.DishIngredients.RemoveAll(x => x.IngredientId.Equals(ingredient.Id));
                    }
                }

                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
