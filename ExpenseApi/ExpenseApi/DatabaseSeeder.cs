using ExpenseApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi
{
    public static class DatabaseSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed currencies
            var usd = new Currency
            {
                Id = Guid.NewGuid(),
                Code = "USD",
                Name = "U.S. Dollar",
                Symbol = "$"
            };

            var rub = new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RUB",
                Name = "Russian Ruble",
                Symbol = "₽"
            };

            modelBuilder.Entity<Currency>().HasData(usd, rub);

            // Seed users
            var starkAnthony = new User
            {
                Id = Guid.NewGuid(),
                LastName = "Stark",
                FirstName = "Anthony",
                CurrencyId = usd.Id
            };

            var romanovaNatasha = new User
            {
                Id = Guid.NewGuid(),
                LastName = "Romanova",
                FirstName = "Natasha",
                CurrencyId = rub.Id
            };

            modelBuilder.Entity<User>().HasData(starkAnthony, romanovaNatasha);

            // Seed random expenses
            var random = new Random();
            var expenses = new List<Expense>
            {
                new() {
                    Id = Guid.NewGuid(),
                    UserId = starkAnthony.Id,
                    Date = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    Type = (ExpenseType)random.Next(0, 3),
                    Amount = random.NextDouble() * 100 + 50,
                    CurrencyId = usd.Id,
                    Comment = "Business meeting"
                },
                new() {
                    Id = Guid.NewGuid(),
                    UserId = romanovaNatasha.Id,
                    Date = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    Type = (ExpenseType)random.Next(0, 3),
                    Amount = random.NextDouble() * 100 + 50,
                    CurrencyId = rub.Id,
                    Comment = "Travel expense"
                },
                new() {
                    Id = Guid.NewGuid(),
                    UserId = starkAnthony.Id,
                    Date = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    Type = (ExpenseType)random.Next(0, 3),
                    Amount = random.NextDouble() * 100 + 50,
                    CurrencyId = usd.Id,
                    Comment = "Client dinner"
                },
                new() {
                    Id = Guid.NewGuid(),
                    UserId = romanovaNatasha.Id,
                    Date = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    Type = (ExpenseType)random.Next(0, 3),
                    Amount = random.NextDouble() * 100 + 50,
                    CurrencyId = rub.Id,
                    Comment = "Hotel stay"
                }
            };

            modelBuilder.Entity<Expense>().HasData(expenses);
        }
    }
}