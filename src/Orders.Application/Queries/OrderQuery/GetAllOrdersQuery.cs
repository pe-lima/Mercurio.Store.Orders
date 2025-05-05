using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Queries.OrderQuery
{
    public record GetAllOrdersQuery(bool IncludeInactive = false);
}
