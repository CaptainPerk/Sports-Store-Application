using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => _context.Orders.Include(order => order.Lines).ThenInclude(line => line.Product);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(line => line.Product));

            if (order.OrderID == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }
    }
}