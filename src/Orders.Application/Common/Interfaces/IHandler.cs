using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Common.Interfaces
{
    public interface IHandler<TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }

    public interface IHandler<in TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}
