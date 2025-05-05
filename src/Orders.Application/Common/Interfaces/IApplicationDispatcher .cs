using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Common.Interfaces
{
    public interface IApplicationDispatcher
    {
        Task<TResult> SendAsync<TRequest, TResult>(TRequest request);
    }
}
