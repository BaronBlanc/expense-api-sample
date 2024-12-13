using ExpenseApi.Model;
using ExpenseApi.Specification;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpenseApi.Repository
{
    public class ExpenseRepository(ExpenseDbContext context) : Repository<Expense>(context), IExpenseRepository
    {
        private readonly ExpenseDbContext _context = context;

        public async Task<IEnumerable<Expense>> GetAllByUserIdSortedAsync(Guid userId, ISpecification<Expense> specification, CancellationToken cancellationToken)
        {
            var query = _context.Set<Expense>().Where(e => e.UserId == userId).
                Include(a => a.Currency).AsQueryable();
            if (specification != null)
            {
                query = specification.Apply(query);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> ExpenseExistsAsync(ISpecification<Expense> specification, CancellationToken cancellationToken)
        {
            return await _context.Set<Expense>()
                .AnyAsync(specification.ToExpression(), cancellationToken);
        }
    }
}
