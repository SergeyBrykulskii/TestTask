using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUser()
        {
            /* algorith with Nlog(N) time complexity, but with one database call
            var userWithMostOrders = await _context.Users
                .OrderByDescending(u => u.Orders.Count)
                .FirstOrDefaultAsync();
            */

            // I chose this option, N time complexity
            var maxNumOfOrders = _context.Users.Max(u => u.Orders.Count);
            var userWithMostOrders = await _context.Users
                .Where(u => u.Orders.Count == maxNumOfOrders)
                .FirstOrDefaultAsync();

            return userWithMostOrders;
        }

        public async Task<List<User>> GetUsers()
        {
            var inactiveUsers = await _context.Users
                .Where(u => u.Status == UserStatus.Inactive)
                .ToListAsync();

            return inactiveUsers;
        }
    }
}
