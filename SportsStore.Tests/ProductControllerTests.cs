using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
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

            var productController = new ProductController(_mockProductRepository.Object) {PageSize = 3};

            //Act
            var result = productController.List(2).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] productArray = result.Products.ToArray();
            Assert.True(productArray.Length == 2);
            Assert.Equal("P4", productArray[0].Name);
            Assert.Equal("P5", productArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
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

            var productController = new ProductController(_mockProductRepository.Object) {PageSize = 3};

            //Act
            var result = productController.List(2).ViewData.Model as ProductsListViewModel;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
