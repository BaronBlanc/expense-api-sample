using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Repository
{
    public class UnitOfWork(ExpenseDbContext context, IExpenseRepository expenseRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository) : IUnitOfWork
    {
        private readonly ExpenseDbContext _context = context;

        public IExpenseRepository Expenses { get; } = expenseRepository;
        public IUserRepository Users { get; } = userRepository;
        public ICurrencyRepository Currencies { get; } = currencyRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
