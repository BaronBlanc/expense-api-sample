using ExpenseApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Repository
{
    public class UserRepository(ExpenseDbContext context) : Repository<User>(context), IUserRepository
    {
        private readonly ExpenseDbContext _context = context;

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Set<User>().FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToArrayAsync();
        }

        public async Task<User> GetByCurrencyIdAsync(Guid currencyId)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.CurrencyId == currencyId);
        }

    }
}
