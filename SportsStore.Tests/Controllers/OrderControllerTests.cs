using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var controller = new OrderController(mockOrderRepository.Object, cart);

            //Act
            var result = controller.Checkout(new Order()) as RedirectToActionResult;

            //Assert
            mockOrderRepository.Verify(repository => repository.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }

        [Fact]
        public void Cannot_Checkout_With_Empty_Cart()
        {
            //Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var cart = new Cart();
            var controller = new OrderController(mockOrderRepository.Object, cart);

            //Act
            var result = controller.Checkout(new Order()) as ViewResult;

            //Assert
            mockOrderRepository.Verify(repository => repository.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_With_Invalid_Shipping_Details()
        {
            //Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var controller = new OrderController(mockOrderRepository.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            //Act
            var result = controller.Checkout(new Order()) as ViewResult;

            //Assert
            mockOrderRepository.Verify(repository => repository.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }
    }
}
