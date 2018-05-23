using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Panginate()
        {
            //Arrange
            Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();

            _mockProductRepository.Setup(m => m.Products).Returns(new[]
            {
                new Product{ ProductID = 1, Name = "P1"},
                new Product{ ProductID = 2, Name = "P2"},
                new Product{ ProductID = 3, Name = "P3"},
                new Product{ ProductID = 4, Name = "P4"},
                new Product{ ProductID = 5, Name = "P5"}
            }.AsQueryable());

            var productController = new ProductController(_mockProductRepository.Object);
            productController.PageSize = 3;

            //Act
            var result = productController.List(2).ViewData.Model as IEnumerable<Product>;

            //Assert
            Product[] productArray = result.ToArray();
            Assert.True(productArray.Length == 2);
            Assert.Equal("P4", productArray[0].Name);
            Assert.Equal("P5", productArray[1].Name);
        }
    }
}
