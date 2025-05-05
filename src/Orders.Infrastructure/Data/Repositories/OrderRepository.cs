using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;
using Orders.Infrastructure.Data.Context;

namespace Orders.Infrastructure.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetByBuyerIdAsync(Guid buyerId) =>
            await _dbSet
                .Include(o => o.Items)
                .Where(o => o.BuyerId == buyerId)
                .ToListAsync();

        public async Task<Order?> GetWithItemsAsync(Guid orderId) =>
            await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<IEnumerable<Order>> GetWithItemsAsync() =>
            await _dbSet
                .Include(o => o.Items)
                .ToListAsync();
    }
}
