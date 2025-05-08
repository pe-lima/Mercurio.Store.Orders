using Moq;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.Handlers.OrderHandler;
using Orders.CrossCutting.Exceptions;
using Orders.Domain.Entities;
using Orders.Domain.Enums;
using Orders.Domain.Interfaces.Repositories;
using System.Net;
using Xunit;

namespace Orders.Tests.Application.Handlers.OrderHandler
{
    public class DeleteOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly DeleteOrderCommandHandler _handler;

        public DeleteOrderCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();

            _mockUnitOfWork.Setup(x => x.Order).Returns(_mockOrderRepo.Object);

            _handler = new DeleteOrderCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Should_Throw_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            var command = new DeleteOrderCommand(Guid.NewGuid());

            _mockOrderRepo
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Order?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.HandleAsync(command));

            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.Equal("Order not found.", ex.Message);
        }

        [Fact]
        public async Task Should_Throw_When_Order_Is_Not_Pending()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderItem>());
            order.MarkAsPaid(); // Status vira Paid

            var command = new DeleteOrderCommand(order.Id);

            _mockOrderRepo
                .Setup(r => r.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.HandleAsync(command));

            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.Equal("Only pending orders can be deleted.", ex.Message);
        }

        [Fact]
        public async Task Should_Delete_Order_Successfully()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderItem>());
            var command = new DeleteOrderCommand(order.Id);

            _mockOrderRepo
                .Setup(r => r.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            _mockOrderRepo.Setup(r => r.Update(order));
            
            _mockUnitOfWork
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            Assert.False(order.IsActive);
            _mockOrderRepo.Verify(r => r.Update(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
