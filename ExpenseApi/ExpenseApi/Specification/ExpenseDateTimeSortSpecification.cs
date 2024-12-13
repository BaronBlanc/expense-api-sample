using ExpenseApi.Commands;
using ExpenseApi.Model;
using System.Linq.Expressions;

namespace ExpenseApi.Specification
{
    public class ExpenseDateTimeSortSpecification : SortSpecification<Expense, DateTime>
    {
        public ExpenseDateTimeSortSpecification(bool ascending = true)
            : base(CreateSortExpression(), ascending) 
        {
        }

        private static Expression<Func<Expense, DateTime>> CreateSortExpression()
        {
            return e => e.Date;
        }
    }
}