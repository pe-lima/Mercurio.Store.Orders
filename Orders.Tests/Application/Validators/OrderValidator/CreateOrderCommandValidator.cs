using FluentAssertions;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.DTOs.Order;
using Orders.Application.Validators.OrderValidator;
using Xunit;

namespace Orders.Tests.Application.Validators.OrderValidator
{
    public class CreateOrderCommandValidatorTests
    {
        private readonly CreateOrderCommandValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Items_Is_Empty()
        {
            var command = new CreateOrderCommand(Guid.NewGuid(), new List<OrderItemDto>());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items");
        }

        [Fact]
        public void Should_Pass_With_Valid_Data()
        {
            var command = new CreateOrderCommand(
                Guid.NewGuid(),
                new List<OrderItemDto> { new(Guid.NewGuid(), "Produto", 100, 2) });

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }
}
