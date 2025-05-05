using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Common.Interfaces;
namespace Orders.Application.Common.Implementations
{
    public class ApplicationDispatcher : IApplicationDispatcher
    {
        private readonly IServiceProvider _provider;

        public ApplicationDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> SendAsync<TRequest, TResult>(TRequest request)
        {
            var handler = _provider.GetRequiredService<IHandler<TRequest, TResult>>();
            return await handler.HandleAsync(request);
        }
    }
}
