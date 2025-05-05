using Orders.Application.DTOs.Order;

namespace Orders.Application.Commands.OrderCommand
{
    public record CreateOrderCommand(
        Guid BuyerId, 
        List<OrderItemDto> Items);
}
