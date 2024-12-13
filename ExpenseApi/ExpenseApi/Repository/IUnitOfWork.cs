namespace ExpenseApi.Repository
{
    public interface IUnitOfWork
    {
        IExpenseRepository Expenses { get; }
        IUserRepository Users { get; }
        ICurrencyRepository Currencies { get; }
        Task SaveAsync();
    }
}
