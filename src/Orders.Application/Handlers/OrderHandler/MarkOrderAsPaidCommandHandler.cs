using Microsoft.VisualBasic;
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
    public class MarkOrderAsPaidCommandHandler : IHandler<MarkOrderAsPaidCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarkOrderAsPaidCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(MarkOrderAsPaidCommand command)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(command.OrderId) ??
                throw new GlobalException("Order not found.", HttpStatusCode.NotFound);

            if (order.Status != OrderStatus.Pending)
                throw new GlobalException("Only pending orders can be paid.", HttpStatusCode.BadRequest);

            order.MarkAsPaid();

            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
