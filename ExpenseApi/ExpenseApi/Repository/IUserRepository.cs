using ExpenseApi.Model;

namespace ExpenseApi.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetByCurrencyIdAsync(Guid currencyId);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
