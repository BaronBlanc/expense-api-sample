using ExpenseApi.Model;
using System.Linq.Expressions;

namespace ExpenseApi.Specification
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
        Expression<Func<Expense, bool>> ToExpression();

    }

}
