using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IHandler<CreateOrderCommand, OrderDto> _createOrderHandler;

    public OrdersController(IHandler<CreateOrderCommand, OrderDto> createOrderHandler)
    {
        _createOrderHandler = createOrderHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        if (command.Items == null || command.Items.Count == 0)
            return BadRequest("Order must contain at least one item.");

        var orderDto = await _createOrderHandler.HandleAsync(command);

        return CreatedAtAction(nameof(GetById), new { id = orderDto.Id }, orderDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok($"Consulta por ID {id} ainda não implementada.");
    }
}
