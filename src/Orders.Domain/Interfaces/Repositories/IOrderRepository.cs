using Orders.Domain.Entities;

namespace Orders.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByBuyerIdAsync(Guid buyerId);
        Task<Order?> GetWithItemsAsync(Guid orderId);
        Task<IEnumerable<Order>> GetWithItemsAsync();
    }
}
