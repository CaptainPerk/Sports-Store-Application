using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            var _mockUrlHelper = new Mock<IUrlHelper>();
            _mockUrlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var _mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            _mockUrlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(_mockUrlHelper.Object);

            var pageLinkTagHelper = new PageLinkTagHelper(_mockUrlHelperFactory.Object)
            {
                PageModel = new PagingInfo
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };

            var tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

            var _mockTagHelperContent = new Mock<TagHelperContent>();

            var tagHelperOutput = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(_mockTagHelperContent.Object));

            //Act
            pageLinkTagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert
            Assert.Equal(@"<a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a>", tagHelperOutput.Content.GetContent());
        }
    }
}
