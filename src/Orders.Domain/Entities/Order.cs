using Orders.Domain.Enums;

namespace Orders.Domain.Entities
{
    public class Order : Entity
    {
        public Guid BuyerId { get; private set; }
        public List<OrderItem> Items { get; private set; } = new ();
        public decimal Total => Items.Sum(i => i.TotalPrice);
        public OrderStatus Status { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }


        protected Order() { }

        public Order(Guid buyerId, List<OrderItem> items)
        {
            BuyerId = buyerId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Status = OrderStatus.Pending;
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Pending) throw new InvalidOperationException("Order cannot be paid.");

            Status = OrderStatus.Paid;
            PaidAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != OrderStatus.Pending) throw new InvalidOperationException("Order cannot be cancelled.");

            Status = OrderStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
        }
    }
}
