using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            var cartLine = lineCollection.FirstOrDefault(line => line.Product.ProductID == product.ProductID);

            if (null == cartLine)
            {
                lineCollection.Add(new CartLine {Product = product, Quantity = quantity});
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => lineCollection.RemoveAll(line => line.Product.ProductID == product.ProductID);

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(line => line.Product.Price * line.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;

    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
