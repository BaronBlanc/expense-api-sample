using ExpenseApi.Model;

namespace ExpenseApi.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(TEntity currency, CancellationToken cancellationToken);
        Task SaveAsync();
    }
}
