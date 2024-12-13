using ExpenseApi.Commands;
using ExpenseApi.Model;
using System.Linq.Expressions;

namespace ExpenseApi.Specification
{
    public class ExpenseAmountSortSpecification : SortSpecification<Expense, double>
    {
        public ExpenseAmountSortSpecification(bool ascending = true)
            : base(CreateSortExpression(), ascending) 
        {
        }

        private static Expression<Func<Expense, double>> CreateSortExpression()
        {
            return e => e.Amount;
        }
    }
}