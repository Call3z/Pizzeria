using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pizzeria.Models;
using Pizzeria.Services;
using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaUnitTests
{
    [TestClass]
    public class CartServiceUnitTests
    {
        [TestMethod]
        public void OrderTotal_VesuvioOriginal_Returns_75SEK()
        {
            var dishes = new List<CartDish>()
            {
                new CartDish()
                {
                    Id = Guid.NewGuid(),
                    Name = "Vesuvio",
                    Price = 75,
                    IncludedIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Cheese",
                            Price = 2,
                            Selected = true
                        },
                        new IngredientViewModel()
                        {
                            Id = 2,
                            Name = "Ham",
                            Price = 2,
                            Selected = true
                        },
                        new IngredientViewModel()
                        {
                            Id = 3,
                            Name = "Tomato",
                            Price = 2,
                            Selected = true
                        }
                    }
                }
            };

            var mockAccessor = new Mock<IHttpContextAccessor>();
            var mockCart = new Mock<CartService>(mockAccessor.Object);

            mockCart.Setup(x => x.GetAllDishes()).Returns(dishes);

            var actual = mockCart.Object.OrderTotal();

            Assert.IsTrue(actual == 75);
        }

        [TestMethod]
        public void OrderTotal_VesuvioExtraIngredients_Returns_85SEK()
        {
            var dishes = new List<CartDish>()
            {
                new CartDish()
                {
                    Id = Guid.NewGuid(),
                    Name = "Vesuvio",
                    Price = 75,
                    IncludedIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Cheese",
                            Price = 2,
                            Selected = true
                        },
                        new IngredientViewModel()
                        {
                            Id = 2,
                            Name = "Ham",
                            Price = 2,
                            Selected = true
                        },
                        new IngredientViewModel()
                        {
                            Id = 3,
                            Name = "Tomato",
                            Price = 2,
                            Selected = true
                        }
                    },
                    ExtraIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Rocket",
                            Price = 10,
                            Selected = true
                        }
                    }
                }
            };

            var mockAccessor = new Mock<IHttpContextAccessor>();
            var mockCart = new Mock<CartService>(mockAccessor.Object);

            mockCart.Setup(x => x.GetAllDishes()).Returns(dishes);

            var actual = mockCart.Object.OrderTotal();

            Assert.IsTrue(actual == 85);
        }

        [TestMethod]
        public void OrderTotal_VesuvioExcludedStandardIngredients_Returns_75SEK()
        {
            var dishes = new List<CartDish>()
            {
                new CartDish()
                {
                    Id = Guid.NewGuid(),
                    Name = "Vesuvio",
                    Price = 75,
                    IncludedIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Cheese",
                            Price = 2,
                            Selected = false
                        },
                        new IngredientViewModel()
                        {
                            Id = 2,
                            Name = "Ham",
                            Price = 2,
                            Selected = false
                        },
                        new IngredientViewModel()
                        {
                            Id = 3,
                            Name = "Tomato",
                            Price = 2,
                            Selected = false
                        }
                    }
                }
            };

            var mockAccessor = new Mock<IHttpContextAccessor>();
            var mockCart = new Mock<CartService>(mockAccessor.Object);

            mockCart.Setup(x => x.GetAllDishes()).Returns(dishes);

            var actual = mockCart.Object.OrderTotal();

            Assert.IsTrue(actual == 75);
        }

        [TestMethod]
        public void OrderTotal_VesuvioExcludedStandardIngredientsWithExtraIngredients_Returns_85SEK()
        {
            var dishes = new List<CartDish>()
            {
                new CartDish()
                {
                    Id = Guid.NewGuid(),
                    Name = "Vesuvio",
                    Price = 75,
                    IncludedIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Cheese",
                            Price = 2,
                            Selected = false
                        },
                        new IngredientViewModel()
                        {
                            Id = 2,
                            Name = "Ham",
                            Price = 2,
                            Selected = false
                        },
                        new IngredientViewModel()
                        {
                            Id = 3,
                            Name = "Tomato",
                            Price = 2,
                            Selected = false
                        }
                    },
                    ExtraIngredients = new List<IngredientViewModel>()
                    {
                        new IngredientViewModel()
                        {
                            Id = 1,
                            Name = "Rocket",
                            Price = 10,
                            Selected = true
                        }
                    }
                }
            };

            var mockAccessor = new Mock<IHttpContextAccessor>();
            var mockCart = new Mock<CartService>(mockAccessor.Object);

            mockCart.Setup(x => x.GetAllDishes()).Returns(dishes);

            var actual = mockCart.Object.OrderTotal();

            Assert.IsTrue(actual == 85);
        }
    }
}
