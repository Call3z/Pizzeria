using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pizzeria.Controllers;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Services;
using Pizzeria.ViewModels;

namespace PizzeriaUnitTests
{
    [TestClass]
    public class OrderControllerUnitTests
    {
        [TestMethod]
        public void Index_Returns_ActionResult()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new ApplicationDbContext(dbOptions);

            var accessorMockmock = new Mock<IHttpContextAccessor>();
            var cartMock = new Mock<ICartService>();
            cartMock.Setup(x => x.CartCreated()).Returns(false);

            var controller = new OrderController(dbContext, cartMock.Object);


            //Act
            var actual = controller.Index();

            //Assert
            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }

        [TestMethod]
        public void Edit_Returns_CartDish_Model()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new ApplicationDbContext(dbOptions);

            var guid = Guid.NewGuid();

            var mock = new Mock<ICartService>();
            mock.Setup(x => x.GetDish(guid)).Returns(new CartDish(){Name = "Calzone", Id = guid, ExtraIngredients = new List<IngredientViewModel>()});

            var controller = new OrderController(dbContext, mock.Object);

            var actual = (CartDish)((ViewResult)controller.Edit(guid)).Model;

            Assert.IsTrue(actual.Name.Equals("Calzone"));
        }
    }
}
