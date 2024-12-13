using ExpenseApi.Model;
using System.Linq.Expressions;

namespace ExpenseApi.Specification
{
    public class SortSpecification<T, T1> : ISpecification<T>
    {
        private readonly Func<IQueryable<T>, IOrderedQueryable<T>> _sortExpression;

        public SortSpecification(Expression<Func<T, T1>> sortBy, bool ascending = true)
        {
            _sortExpression = ascending
                ? (query) => query.OrderBy(sortBy)
                : (query) => query.OrderByDescending(sortBy);
        }

        public IQueryable<T> Apply(IQueryable<T> query)
        {
            return _sortExpression(query);
        }

        public Expression<Func<Expense, bool>> ToExpression()
        {
            throw new NotImplementedException();
        }
    }

}
