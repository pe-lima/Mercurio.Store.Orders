using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Domain.Entities;

namespace Orders.Application.Mappers.OrderMapper;

public class OrderMapper : IMapper<Order, OrderDto>
{
    public OrderDto ToTarget(Order order)
    {
        var itemDtos = order.Items.Select(item => new OrderItemDto(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Quantity
        )).ToList();

        return new OrderDto(
            order.Id,
            order.BuyerId,
            order.Status.ToString(),
            order.CreatedAt,
            itemDtos
        );
    }

    public Order ToSource(OrderDto dto)
    {
        var items = dto.Items.Select(item => new OrderItem(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Quantity
        )).ToList();

        return new Order(dto.BuyerId, items);
    }
}
