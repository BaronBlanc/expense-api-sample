using AutoMapper;
using ExpenseApi.Commands;
using ExpenseApi.Exceptions;
using ExpenseApi.Model;

namespace ExpenseApi.Tools
{
    public class ExpenseTypeResolver : IValueResolver<CreateExpenseCommand, Expense, ExpenseType>
    {
        public ExpenseType Resolve(CreateExpenseCommand source, Expense destination, ExpenseType destMember, ResolutionContext context)
        {
            if (!Enum.TryParse<ExpenseType>(source.Type, true, out var expenseType))
            {
                throw new FunctionalException(FunctionalError.CANNOT_INTERPRET_TYPE, StatusCodes.Status400BadRequest);
            }

            return expenseType;
        }
    }
}