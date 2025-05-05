using Orders.Application.Commands.OrderCommand;
using Orders.Application.Common.Interfaces;
using Orders.CrossCutting.Exceptions;
using Orders.Domain.Enums;
using Orders.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Handlers.OrderHandler
{
    public class DeleteOrderCommandHandler : IHandler<DeleteOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteOrderCommand command)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(command.Id) ??
                throw new GlobalException("Order not found.", HttpStatusCode.NotFound);

            if (order.Status != OrderStatus.Pending)
                throw new GlobalException("Only pending orders can be deleted.", HttpStatusCode.BadRequest);

            order.SetDelete();

            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
