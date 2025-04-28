using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.DTOs.Order
{
    public record OrderItemDto(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity
    );
}
