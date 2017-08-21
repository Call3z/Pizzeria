﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;

namespace Pizzeria.Data
{
    public static class Seed
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var adminRole = new IdentityRole { Name = "Admin" };
            var adminRoleResult = roleManager.CreateAsync(adminRole).Result;

            var memberRole = new IdentityRole { Name = "Member" };
            var memberRoleResult = roleManager.CreateAsync(memberRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Test123#").Result;

            var user = new ApplicationUser();
            user.Email = "user@test.com";
            user.UserName = "user@test.com";
            var p = userManager.CreateAsync(user, "Test123#").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");
            userManager.AddToRoleAsync(user, "Member");

            if (!context.Dishes.Any())
            {

                var vesuvio = new Dish() { Name = "Vesuvio", Price = 75 };
                var hawaii = new Dish() { Name = "Hawaii", Price = 80 };
                var margaritha = new Dish() { Name = "Margaritha", Price = 80 };

                var cheese = new Ingredient { Name = "Cheese" };
                var tomato = new Ingredient { Name = "Tomato" };
                var ham = new Ingredient { Name = "Ham" };

                var vesuvioCheese = new DishIngredient { Dish = vesuvio, Ingredient = cheese };
                var vesuvioTomato = new DishIngredient { Dish = vesuvio, Ingredient = tomato };
                var vesuvioHam = new DishIngredient { Dish = vesuvio, Ingredient = ham };

                vesuvio.DishIngredients = new List<DishIngredient>();
                vesuvio.DishIngredients.Add(vesuvioCheese);
                vesuvio.DishIngredients.Add(vesuvioTomato);
                vesuvio.DishIngredients.Add(vesuvioHam);

                context.AddRange(vesuvioCheese, vesuvioTomato, vesuvioHam);
                context.AddRange(vesuvio, hawaii, margaritha);
                context.SaveChanges();
            }

        }
    }
}