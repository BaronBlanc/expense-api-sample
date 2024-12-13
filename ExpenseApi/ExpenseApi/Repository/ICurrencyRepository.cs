using ExpenseApi.Model;

namespace ExpenseApi.Repository
{
    public interface ICurrencyRepository: IRepository<Currency>
    {
        Task<Currency> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<Currency> GetBySymbolAsync(string symbol, CancellationToken cancellationToken);
    }
}
