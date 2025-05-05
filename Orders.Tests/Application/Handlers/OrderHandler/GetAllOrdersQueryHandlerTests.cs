using Moq;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Handlers.OrderHandler;
using Orders.Application.Mappers.OrderMapper;
using Orders.Application.Queries.OrderQuery;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces.Repositories;
using Xunit;

namespace Orders.Tests.Application.Handlers.OrderHandler
{
    public class GetAllOrdersQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMapper<Order, OrderDto>> _mockMapper;
        private readonly GetAllOrdersQueryHandler _handler;

        public GetAllOrdersQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper<Order, OrderDto>>();

            _mockUnitOfWork
                .Setup(u => u.Order)
                .Returns(_mockOrderRepository.Object);

            _handler = new GetAllOrdersQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOnlyActiveOrders_WhenIncludeInactiveIsFalse()
        {
            // Arrange
            var activeOrder = new Order(Guid.NewGuid(), new List<OrderItem> { new(Guid.NewGuid(), "Product1", 10, 2) });
            var inactiveOrder = new Order(Guid.NewGuid(), new List<OrderItem> { new(Guid.NewGuid(), "Product2", 20, 1) });
            inactiveOrder.SetDelete(); // Marca como IsActive = false

            var orders = new List<Order> { activeOrder, inactiveOrder };

            _mockOrderRepository
                .Setup(r => r.GetWithItemsAsync())
                .ReturnsAsync(orders);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Order>()))
                .Returns<Order>(o =>
                    new OrderDto(o.Id, o.BuyerId, "Pending", o.CreatedAt, new(), o.IsActive));

            // Act
            var result = await _handler.HandleAsync(new GetAllOrdersQuery(false));

            // Assert
            Assert.Single(result);
            Assert.True(result.All(o => o.IsActive));
        }

        [Fact]
        public async Task Handle_ShouldReturnAllOrders_WhenIncludeInactiveIsTrue()
        {
            // Arrange
            var activeOrder = new Order(Guid.NewGuid(), new List<OrderItem> { new(Guid.NewGuid(), "Product1", 10, 2) });
            var inactiveOrder = new Order(Guid.NewGuid(), new List<OrderItem> { new(Guid.NewGuid(), "Product2", 20, 1) });
            
            inactiveOrder.SetDelete();

            var orders = new List<Order> { activeOrder, inactiveOrder };

            _mockOrderRepository
                .Setup(r => r.GetWithItemsAsync())
                .ReturnsAsync(orders);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Order>()))
                .Returns<Order>(o =>
                    new OrderDto(o.Id, o.BuyerId, "Pending", o.CreatedAt, new(), o.IsActive));

            // Act
            var result = await _handler.HandleAsync(new GetAllOrdersQuery(true));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => !o.IsActive);
        }
    }
}
