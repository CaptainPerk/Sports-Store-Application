﻿using Microsoft.AspNetCore.Mvc;
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

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
