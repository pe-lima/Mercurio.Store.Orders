using Orders.Domain.Interfaces.Repositories;
using Orders.Infrastructure.Data.Context;
using Orders.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Order = new OrderRepository(_context);
        }

        public IOrderRepository Order { get; }
        
        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
