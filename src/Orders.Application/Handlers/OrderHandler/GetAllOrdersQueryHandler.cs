using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Queries.OrderQuery;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Handlers.OrderHandler
{
    public class GetAllOrdersQueryHandler : IHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<Order, OrderDto> _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper<Order, OrderDto> orderMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = orderMapper;
        }

        public async Task<List<OrderDto>> HandleAsync(GetAllOrdersQuery query)
        {
            var orders = await _unitOfWork.Order.GetWithItemsAsync();

            var filtered = query.IncludeInactive 
                ? orders 
                : orders.Where(o => o.IsActive);
            
            return filtered
                    .Select(order => _mapper.ToTarget(order))
                    .ToList();
        }
    }
}
