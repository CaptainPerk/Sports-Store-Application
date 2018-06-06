using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.ViewComponents
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repository => repository.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"}
            }.AsQueryable());

            var target = new NavigationMenuViewComponent(mockProductRepository.Object);

            //Act
            var results = ((IEnumerable<string>) (target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            //Assert
            Assert.True(new[] {"Apples", "Oranges", "Plums"}.SequenceEqual(results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //Arrange
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repository => repository.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"}
            }.AsQueryable());

            var target = new NavigationMenuViewComponent(mockProductRepository.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext {RouteData = new RouteData()}
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            //Act
            string result = (string) (target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
