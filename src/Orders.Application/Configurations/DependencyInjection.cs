using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Handlers.OrderHandler;
using Orders.Application.Mappers.OrderMapper;
using Orders.Application.Validators.OrderValidator;
using Orders.Domain.Entities;
using Orders.Application.Common.Implementations;
using Orders.Application.Queries.OrderQuery;

namespace Orders.Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // (mappers) 
            services.AddScoped<IMapper<Order, OrderDto>, OrderMapper>();

            //handlers
            services.AddScoped<IHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
            services.AddScoped<IHandler<GetAllOrdersQuery, List<OrderDto>>, GetAllOrdersQueryHandler>();
            services.AddScoped<IHandler<GetOrdersByIdQuery, OrderDto>, GetOrdersByIdQueryHandler>();

            // (validators)
            services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
            services.AddFluentValidationAutoValidation();

            // (dispatcher)
            services.AddScoped<IApplicationDispatcher, ApplicationDispatcher>();

            return services;
        }
    }
}
