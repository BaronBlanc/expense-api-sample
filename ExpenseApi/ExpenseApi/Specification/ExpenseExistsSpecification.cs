using ExpenseApi.Model;
using System.Linq.Expressions;

namespace ExpenseApi.Specification
{
    public class ExpenseExistsSpecification(Guid userId, DateTime date, double amount) : ISpecification<Expense>
    {
        private readonly Guid _userId = userId;
        private readonly DateTime _date = date;
        private readonly double _amount = amount;

        public IQueryable<Expense> Apply(IQueryable<Expense> query)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Expense, bool>> ToExpression()
        {
            return expense => expense.UserId == _userId &&
                             expense.Date == _date &&
                             expense.Amount == _amount;
        }
    }

}
