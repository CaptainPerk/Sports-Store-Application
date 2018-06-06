using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Controllers
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
            var result = productController.List(null, 2).ViewData.Model as ProductsListViewModel;

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
            var result = productController.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Arrange
            var _mockProductRepository = new Mock<IProductRepository>();
            _mockProductRepository.Setup(repository => repository.Products).Returns((new[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }).AsQueryable());

            var productController = new ProductController(_mockProductRepository.Object) {PageSize = 3};

            //Act
            var result = (productController.List("Cat2").ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            var _mockProductRepository = new Mock<IProductRepository>();
            _mockProductRepository.Setup(repository => repository.Products).Returns(new[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }.AsQueryable());

            var productController = new ProductController(_mockProductRepository.Object) { PageSize = 3 };

            Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //Act
            int? result1 = GetModel(productController.List("Cat1"))?.PagingInfo.TotalItems;
            int? result2 = GetModel(productController.List("Cat2"))?.PagingInfo.TotalItems;
            int? result3 = GetModel(productController.List("Cat3"))?.PagingInfo.TotalItems;
            int? result4 = GetModel(productController.List(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(1, result3);
            Assert.Equal(5, result4);
        }
    }
}
