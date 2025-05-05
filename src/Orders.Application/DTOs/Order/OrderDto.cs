using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.DTOs.Order
{
    public record OrderDto(
        Guid Id,
        Guid BuyerId,
        string Status,
        DateTime CreatedAt,
        List<OrderItemDto> Items,
        bool IsActive
    );
}
