using FluentValidation;
using Orders.Application.Commands.OrderCommand;

namespace Orders.Application.Validators.OrderValidator;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty()
            .WithMessage("O identificador do comprador é obrigatório.");

        RuleFor(x => x.Items)
            .NotNull()
            .WithMessage("A lista de itens não pode ser nula.")
            .Must(items => items.Count != 0)
            .WithMessage("A lista de itens deve conter ao menos um item.");

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("O ID do produto é obrigatório.");

            items.RuleFor(i => i.ProductName)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.");

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("A quantidade deve ser maior que zero.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0)
                .WithMessage("O preço unitário deve ser maior que zero.");
        });
    }
}
