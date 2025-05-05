using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Queries.OrderQuery;
using Orders.CrossCutting.Exceptions;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;
using System.Net;

namespace Orders.Application.Handlers.OrderHandler
{
    public class GetOrdersByIdQueryHandler : IHandler<GetOrdersByIdQuery, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<Order, OrderDto> _mapper;
        public GetOrdersByIdQueryHandler(IUnitOfWork unitOfWork, IMapper<Order, OrderDto> orderMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = orderMapper;
        }
        public async Task<OrderDto> HandleAsync(GetOrdersByIdQuery query)
        {
            var order = await _unitOfWork.Order.GetWithItemsAsync(query.Id) ??
                throw new GlobalException("Person not found.", HttpStatusCode.NotFound);

            return _mapper.ToTarget(order);
        }
    }
}
