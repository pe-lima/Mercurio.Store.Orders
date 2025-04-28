using Microsoft.EntityFrameworkCore;
using Orders.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public void Update(T entity) =>
            _dbSet.Update(entity);

        public void Remove(T entity) =>
            _dbSet.Remove(entity);
    }
}
