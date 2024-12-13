using AutoMapper;
using ExpenseApi.Commands;
using ExpenseApi.Exceptions;
using ExpenseApi.Model;
using ExpenseApi.Tools;
using Microsoft.AspNetCore.Http;
using Xunit;
using Assert = Xunit.Assert;

namespace ExpenseApi.Tests
{
    public class MapperProfileTests
    {
        private readonly IMapper _mapper;

        public MapperProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MapperConfiguration_IsValid()
        {
            // Ensure the AutoMapper configuration is valid
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_CreateExpenseCommand_To_Expense()
        {
            // Arrange
            var createExpenseCommand = new CreateExpenseCommand
            {
                UserId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.UtcNow,
                Currency = "$",
                Type = "Restaurant",
                Comment = "lunch",
            };

            // Act
            var expense = _mapper.Map<Expense>(createExpenseCommand);

            // Assert
            Assert.NotNull(expense);
            Assert.Equal(createExpenseCommand.UserId, expense.UserId);
            Assert.Equal(createExpenseCommand.Amount, expense.Amount);
            Assert.Equal(createExpenseCommand.Date, expense.Date);
            Assert.Equal((ExpenseType)Enum.Parse(typeof(ExpenseType), createExpenseCommand.Type, true), expense.Type);
            Assert.Equal(createExpenseCommand.Comment, expense.Comment);
            Assert.NotEqual(Guid.Empty, expense.Id); // Ensure a new GUID is generated
        }

        [Fact]
        public void Should_Map_Expense_To_ExpenseView()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };
            var currency = new Currency
            {
                Id = Guid.NewGuid(),
                Symbol = "$"
            };
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                User = user,
                Amount = 150,
                Date = DateTime.UtcNow,
                Currency = currency,
                Type = ExpenseType.Misc,
                Comment = "Car repair",
                CurrencyId = Guid.NewGuid(),
                UserId = user.Id,
            };

            // Act
            var expenseView = _mapper.Map<ExpenseView>(expense);

            // Assert
            Assert.NotNull(expenseView);
            Assert.Equal(expense.Id, expenseView.Id);
            Assert.Equal($"{user.FirstName} {user.LastName}", expenseView.User);
            Assert.Equal(expense.Amount, expenseView.Amount);
            Assert.Equal(expense.Date, expenseView.Date);
            Assert.Equal(currency.Symbol, expenseView.Currency);
            Assert.Equal(expense.Type.ToString(), expenseView.Type);
            Assert.Equal(expense.Comment, expenseView.Comment);
            Assert.Equal(expense.Type.ToString(), expenseView.Type);
            Assert.Equal(expense.Currency.Symbol, expenseView.Currency);
            Assert.Equal($"{expense.User.FirstName} {expense.User.LastName}", expenseView.User);

        }

        [Fact]
        public void Map_Expense_To_CreateExpenseResult_ShouldMapCorrectly()
        {
            // Arrange
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Amount = 150.50,
                Comment = "Dinner with client",
                Date = DateTime.UtcNow,
                Currency = new Currency
                {
                    Id = Guid.NewGuid(),
                    Symbol = "$"
                },
                Type = ExpenseType.Restaurant
            };

            // Act
            var result = _mapper.Map<CreateExpenseResult>(expense);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expense.Currency.Symbol, result.Currency);
            Assert.Equal(expense.Amount, result.Amount);
            Assert.Equal(expense.Type.ToString(), result.Type);
            Assert.Equal(expense.Comment, result.Comment);
            Assert.Equal(expense.Date, result.Date);
        }

        [Fact]
        public void Map_InvalidExpenseType_ShouldThrowAutoMapperMappingException()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Type = "InvalidType", // Invalid type for casting
                Amount = 100,
                Currency = "$",
                Comment = "Test comment"
            };

            // Act & Assert
            var exception = Assert.Throws<AutoMapperMappingException>(() =>
            {
                _mapper.Map<Expense>(command);
            });

            Assert.Equal(FunctionalError.CANNOT_INTERPRET_TYPE, exception.InnerException.Message);
        }
    }
}
