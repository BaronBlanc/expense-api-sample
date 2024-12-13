using ExpenseApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Repository
{
    public class Repository<TEntity>(ExpenseDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
    {
        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Set<TEntity>().FindAsync([id], cancellationToken);
        }

        public async Task AddAsync(TEntity currency, CancellationToken cancellationToken)
        {
            await context.Set<TEntity>().AddAsync(currency, cancellationToken);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
