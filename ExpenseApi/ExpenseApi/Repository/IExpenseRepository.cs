using ExpenseApi.Model;
using ExpenseApi.Specification;
using System.Linq.Expressions;

namespace ExpenseApi.Repository
{
    public interface IExpenseRepository: IRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllByUserIdSortedAsync(Guid userId, ISpecification<Expense> specification, CancellationToken cancellationToken);
        Task<bool> ExpenseExistsAsync(ISpecification<Expense> specification, CancellationToken cancellationToken);
    }
}
