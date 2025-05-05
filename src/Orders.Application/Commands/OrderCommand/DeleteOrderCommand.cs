using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Commands.OrderCommand
{
    public record DeleteOrderCommand (Guid Id);
}
