using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Services;
using Microsoft.AspNetCore.Identity;
using Pizzeria.Models;
using Pizzeria.Models.CheckOutViewModels;

namespace Pizzeria.Controllers
{
    public class PaymentController : Controller
    {
        private ICartService _cartService;
        private UserManager<ApplicationUser> _userManager;

        public PaymentController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Pay()
        {
            if(!_cartService.CartCreated())
            {
                return RedirectToAction("Index", "Home");
            }
            var dishes = _cartService.GetAllDishes();
            var total = _cartService.OrderTotal();

            var loggedInUser =  _userManager.GetUserAsync(User).Result;

            var viewModel = new PaymentViewModel()
            {
                Dishes = dishes,
                OrderTotal = total
            };

            if(loggedInUser != null)
            {
                viewModel.Name = loggedInUser.Name;
                viewModel.City = loggedInUser.City;
                viewModel.Street = loggedInUser.Street;
                viewModel.Zip = loggedInUser.Zip;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Pay(PaymentViewModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Receipt");
            }

            return View(model);
        }

        public IActionResult Receipt()
        {
            ViewData["Total"] = _cartService.OrderTotal();
            return View();
        }
    }
}