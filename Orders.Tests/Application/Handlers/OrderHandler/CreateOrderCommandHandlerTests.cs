using Moq;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Handlers.OrderHandler;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;
using Xunit;

namespace Orders.Tests.Application.Handlers.OrderHandler
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper<Order, OrderDto>> _mockMapper;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper<Order, OrderDto>>();
            _handler = new CreateOrderCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Create_Order_Successfully()
        {
            var command = new CreateOrderCommand(
                Guid.NewGuid(),
                new List<OrderItemDto> { new(Guid.NewGuid(), "Produto", 100, 2) });

            var expectedOrder = new Order(command.BuyerId, command.Items.Select(i =>
                new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity)).ToList());

            var expectedDto = new OrderDto(expectedOrder.Id, command.BuyerId, "Pending", expectedOrder.CreatedAt, new());

            _mockUnitOfWork
                .Setup(x => x.Order.AddAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);
            
            _mockUnitOfWork
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            
            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Order>()))
                .Returns(expectedDto);

            var result = await _handler.HandleAsync(command);

            Assert.NotNull(result);
            Assert.Equal(command.BuyerId, result.BuyerId);
        }
    }
}
