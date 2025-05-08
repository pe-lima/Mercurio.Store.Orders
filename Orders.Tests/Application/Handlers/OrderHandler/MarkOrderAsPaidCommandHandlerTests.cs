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
    public class MarkOrderAsPaidCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly MarkOrderAsPaidCommandHandler _handler;

        public MarkOrderAsPaidCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepo = new Mock<IOrderRepository>();

            _mockUnitOfWork.Setup(x => x.Order).Returns(_mockOrderRepo.Object);

            _handler = new MarkOrderAsPaidCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Should_Pay_Order_Successfully()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderItem>());
            var command = new MarkOrderAsPaidCommand(order.Id);

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
            Assert.Equal(OrderStatus.Paid, order.Status);
            Assert.NotNull(order.PaidAt);

            _mockOrderRepo.Verify(r => r.Update(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Order_Not_Found()
        {
            // Arrange
            var command = new MarkOrderAsPaidCommand(Guid.NewGuid());

            _mockOrderRepo.Setup(r => r.GetByIdAsync(command.OrderId))
                          .ReturnsAsync((Order?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.HandleAsync(command));

            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.Equal("Order not found.", ex.Message);
        }

        [Fact]
        public async Task Should_Throw_When_Order_Not_Pending()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), new List<OrderItem>());
            order.Cancel(); // Status vira Cancelled

            var command = new MarkOrderAsPaidCommand(order.Id);

            _mockOrderRepo.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.HandleAsync(command));

            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.Equal("Only pending orders can be paid.", ex.Message);
        }
    }
}
