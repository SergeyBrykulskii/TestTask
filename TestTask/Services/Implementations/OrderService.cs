using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrder()
        {
            /* algorith with Nlog(N) time complexity, but with one database call
            var orderWithLargestPrice = await _context.Orders
                .OrderByDescending(o => o.Price)
                .FirstOrDefaultAsync();
            */

            // I chose this option, N time complexity
            var maxPrice = _context.Orders.Max(o => o.Price);
            var orderWithLargestPrice = await _context.Orders
                .Where(o => o.Price == maxPrice)
                .FirstOrDefaultAsync();

            return orderWithLargestPrice;
        }

        public async Task<List<Order>> GetOrders()
        {
            var ordersQuantityMoreThanTen = await _context.Orders
                .Where(o => o.Quantity > 10)
                .ToListAsync();
            return ordersQuantityMoreThanTen;
        }
    }
}
