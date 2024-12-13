using ExpenseApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ExpenseApi.Repository
{
    public class CurrencyRepository(ExpenseDbContext context) : Repository<Currency>(context), ICurrencyRepository
    {
        private readonly ExpenseDbContext _context = context;

        public async Task<Currency> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Set<Currency>().FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<Currency> GetBySymbolAsync(string symbol, CancellationToken cancellationToken)
        {
            return await _context.Set<Currency>().FirstOrDefaultAsync(c => c.Symbol == symbol, cancellationToken);
        }
    }

}
