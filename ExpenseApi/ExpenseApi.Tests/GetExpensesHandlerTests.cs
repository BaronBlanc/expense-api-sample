using ExpenseApi.Handlers;
using ExpenseApi.Commands;
using ExpenseApi.Model;
using ExpenseApi.Repository;
using ExpenseApi.Specification;
using Moq;
using Xunit;
using Assert = Xunit.Assert;
using AutoMapper;
using ExpenseApi.Tools;
using ExpenseApi.Exceptions;

namespace ExpenseApi.Tests
{
    public class GetExpensesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetExpensesHandler _handler;
        private readonly IMapper _mapper;

        public GetExpensesHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            _mapper = configuration.CreateMapper();
            _handler = new GetExpensesHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetExpensesQuery(userId, true, "Date");
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal(FunctionalError.USER_NOT_FOUND, exception.Message);
        }

        [Fact]
        public void Handle_BadSortExpression_ThrowsFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var exception = Assert.Throws<FunctionalException>(() => new GetExpensesQuery(userId, true, "InvalidSortExpression"));

            // Act & Assert
            Assert.Equal(FunctionalError.SORT_EXPRESSION_NOT_VALID, exception.Message);
        }

        [Fact]
        public async Task Handle_ValidUser_ReturnsExpenseViewList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetExpensesQuery(userId, true, "Date");
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe", CurrencyId = Guid.NewGuid() };
            var expenses = new List<Expense>
            {
                new() {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Date = DateTime.UtcNow,
                    Type = ExpenseType.Restaurant,
                    Amount = 100.10,
                    CurrencyId = Guid.NewGuid(),
                    Comment = "Lunch",
                    Currency = new Currency()
                    {
                        Name = "USD",
                        Symbol = "$",
                    },
                    User = user
                }
        };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Expenses.GetAllByUserIdSortedAsync(userId, It.IsAny<ExpenseDateTimeSortSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(expenses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var expenseView = result[0];
            Assert.Equal(expenses[0].Id, expenseView.Id);
            Assert.Equal(expenses[0].Date, expenseView.Date);
            Assert.Equal("John Doe", expenseView.User);
            Assert.Equal(expenses[0].Type.ToString(), expenseView.Type);
            Assert.Equal(expenses[0].Amount, expenseView.Amount);
            Assert.Equal(expenses[0].Currency.Symbol, expenseView.Currency);
            Assert.Equal(expenses[0].Comment, expenseView.Comment);
        }

        [Fact]
        public async Task Handle_NoExpenses_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetExpensesQuery(userId, true, "Date");
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe", CurrencyId = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Expenses.GetAllByUserIdSortedAsync(userId, It.IsAny<ExpenseAmountSortSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_SortSpecification_UsesCorrectSpecification()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetExpensesQuery(userId, false, "Amount");
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe", CurrencyId = Guid.NewGuid() };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Expenses.GetAllByUserIdSortedAsync(userId, It.IsAny<ExpenseAmountSortSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}