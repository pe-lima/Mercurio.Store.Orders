using System.Net;
using Moq;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Handlers.OrderHandler;
using Orders.Application.Queries.OrderQuery;
using Orders.CrossCutting.Exceptions;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;

namespace Orders.Tests.Application.Handlers.OrderHandler
{
    public class GetOrdersByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper<Order, OrderDto>> _mockMapper;
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly GetOrdersByIdQueryHandler _handler;

        public GetOrdersByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper<Order, OrderDto>>();

            _mockUnitOfWork.Setup(x => x.Order).Returns(_mockOrderRepo.Object);

            _handler = new GetOrdersByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOrderDto_WhenOrderExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), new List<OrderItem>
            {
                new(Guid.NewGuid(), "Produto Teste", 50, 1)
            });

            var expectedDto = new OrderDto(order.Id, order.BuyerId, "Pending", order.CreatedAt, new(), order.IsActive);

            _mockOrderRepo.Setup(r => r.GetWithItemsAsync(id))
                          .ReturnsAsync(order);

            _mockMapper.Setup(m => m.ToTarget(order))
                       .Returns(expectedDto);

            // Act
            var result = await _handler.HandleAsync(new GetOrdersByIdQuery(id));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenOrderNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockOrderRepo.Setup(r => r.GetWithItemsAsync(id))
                          .ReturnsAsync((Order?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.HandleAsync(new GetOrdersByIdQuery(id)));

            Assert.Equal("Person not found.", ex.Message);
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }
    }
}
