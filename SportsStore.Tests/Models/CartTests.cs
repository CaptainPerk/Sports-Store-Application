using SportsStore.Models;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Models
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //Arrange
            var product1 = new Product{ ProductID = 1, Name = "P1"};
            var product2 = new Product{ ProductID = 2, Name = "P2"};

            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            CartLine[] results = cart.Lines.ToArray();

            //Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(product1, results[0].Product);
            Assert.Equal(product2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };

            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 10);
            CartLine[] results = cart.Lines.OrderBy(line => line.Product.ProductID).ToArray();

            //Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };
            var product3 = new Product { ProductID = 3, Name = "P3" };

            var cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 3);
            cart.AddItem(product3, 5);
            cart.AddItem(product2, 1);

            //Act
            cart.RemoveLine(product2);

            //Assert
            Assert.Equal(0, cart.Lines.Count(line => line.Product == product2));
            Assert.Equal(2, cart.Lines.Count());
        }

        [Fact]
        public void Can_Calculate_Cart_Total()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1", Price = 100M};
            var product2 = new Product { ProductID = 2, Name = "P2", Price = 50M};

            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 3);
            var result = cart.ComputeTotalValue();

            //Assert
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };

            var cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            //Act
            cart.Clear();

            //Assert
            Assert.False(cart.Lines.Any());
        }
    }
}
