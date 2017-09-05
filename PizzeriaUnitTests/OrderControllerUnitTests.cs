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
    }
}
