using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Controllers
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(r => r.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"}, 
                new Product {ProductID = 2, Name = "P2"}, 
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable());

            var controller = new AdminController(mockProductRepository.Object);

            //Act
            var result = GetViewModel<IEnumerable<Product>>(controller.Index())?.ToArray();

            //Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Products()
        {
            //Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(r => r.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable());

            var controller = new AdminController(mockProductRepository.Object);

            //Act
            var p1 = GetViewModel<Product>(controller.Edit(1));
            var p2 = GetViewModel<Product>(controller.Edit(2));
            var p3 = GetViewModel<Product>(controller.Edit(3));

            //Assert
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Products()
        {
            //Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(r => r.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable());

            var controller = new AdminController(mockProductRepository.Object);

            //Act
            var result = GetViewModel<Product>(controller.Edit(4));

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            var mockTempDataDictionary = new Mock<ITempDataDictionary>();

            var controller = new AdminController(mockProductRepository.Object)
            {
                TempData = mockTempDataDictionary.Object
            };

            var product = new Product { Name = "Test" };

            //Act
            var result = controller.Edit(product);

            //Assert
            mockProductRepository.Verify(r => r.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);

        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            //Arrange
            var mockProductRepository = new Mock<IProductRepository>();

            var controller = new AdminController(mockProductRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            var product = new Product { Name = "Test" };

            //Act
            var result = controller.Edit(product);

            //Assert
            mockProductRepository.Verify(r => r.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            //Arrange
            var product = new Product {ProductID = 2, Name = "Test"};

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(r => r.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                product,
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable());

            var controller = new AdminController(mockProductRepository.Object);

            //Act
            controller.Delete(product.ProductID);

            //Assert
            mockProductRepository.Verify(r => r.DeleteProduct(product.ProductID));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
