using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.Application.DTOs.Order;
using Orders.Application.Queries.OrderQuery;
using System.Threading;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IApplicationDispatcher _dispatcher;

    public OrdersController(IApplicationDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        if (command.Items == null || command.Items.Count == 0)
            return BadRequest("Order must contain at least one item.");

        var order = await _dispatcher.SendAsync<CreateOrderCommand, OrderDto>(command);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _dispatcher.SendAsync<GetOrdersByIdQuery, OrderDto>(new (id));
        
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
    {
        var result = await _dispatcher.SendAsync<GetAllOrdersQuery, List<OrderDto>>(new (includeInactive));

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _dispatcher.SendAsync<DeleteOrderCommand>(new(id));
        
        return NoContent();
    }

    [HttpPatch("{id:guid}/pay")]
    public async Task<IActionResult> Pay(Guid id)
    {
        await _dispatcher.SendAsync<MarkOrderAsPaidCommand>(new(id));

        return NoContent();
    }
}
