using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;

namespace Orders.Application.Handlers.OrderHandler
{
    public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<Order, OrderDto> _orderMapper;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper<Order, OrderDto> orderMapper)
        {
            _unitOfWork = unitOfWork;
            _orderMapper = orderMapper;
        }

        public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
        {
            var items = command.Items.Select(i =>
                new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity)
            ).ToList();

            var order = new Order(command.BuyerId, items);

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _orderMapper.ToTarget(order);
        }
    }
}
