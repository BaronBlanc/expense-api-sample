using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ExpenseApi.Commands;
using ExpenseApi.Model;
using ExpenseApi.Repository;
using ExpenseApi.Handlers;
using MediatR;
using ExpenseApi.Specification;
using Assert = Xunit.Assert;
using ExpenseApi.Tools;
using AutoMapper;
using ExpenseApi.Exceptions;
using NSubstitute;

namespace ExpenseApi.Tests
{
    public class CreateExpenseHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly CreateExpenseHandler _handler;
        private readonly IMapper _mapper;

        public CreateExpenseHandlerTests()
        {
            // Mocking the IUnitOfWork and its dependencies
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _expenseRepositoryMock = new Mock<IExpenseRepository>();
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();

            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Expenses).Returns(_expenseRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Currencies).Returns(_currencyRepositoryMock.Object);


            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            _mapper = configuration.CreateMapper();

            // Creating the handler instance
            _handler = new CreateExpenseHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateExpense()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),  // Valid date within range
                Type = "Restaurant",
                Amount = 100,
                Currency = "$",
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = currencyId,
                    Symbol = "$"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _expenseRepositoryMock.Setup(r => r.ExpenseExistsAsync(It.IsAny<ExpenseExistsSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync(command.Currency, It.IsAny<CancellationToken>())).ReturnsAsync(user.Currency);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id); 
            Assert.Equal(command.UserId, result.UserId);
            Assert.Equal(command.Amount, result.Amount);
            Assert.Equal(command.Type, result.Type.ToString());
            _expenseRepositoryMock.Verify(u => u.AddAsync(It.Is<Expense>(e =>
                e.UserId == result.UserId &&
                e.CurrencyId == currencyId &&
                e.Id == result.Id &&
                e.Amount == result.Amount &&
                e.Comment == result.Comment &&
                e.Date == result.Date &&
                e.Type.ToString() == result.Type
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),
                Type = "Misc",
                Amount = 100,
                Currency = "$",
                Comment = "Lunch"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.USER_NOT_FOUND, exception.Message);
        }

        [Fact]
        public async Task Handle_FutureDate_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(1),  // Invalid date (future)
                Type = "Hotel",
                Amount = 100,
                Currency = "$",
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = currencyId,
                    Symbol = "$"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.EXPENSE_DATE_CANNOT_BE_IN_FUTURE, exception.Message);
        }

        [Fact]
        public async Task Handle_OlderThanThreeMonths_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddMonths(-4),  // Invalid date (older than 3 months)
                Type = "Restaurant",
                Amount = 100,
                Currency = "$",
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = currencyId,
                    Symbol = "$"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.EXPENSE_DATE_CANNOT_BE_TOO_OLD, exception.Message);
        }

        [Fact]
        public async Task Handle_MissingComment_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),
                Type = "Misc",
                Amount = 100,
                Currency = "$",
                Comment = null  // Invalid comment (null)
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = currencyId,
                    Symbol = "$"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.COMMENT_REQUIRED, exception.Message);
        }

        [Fact]
        public async Task Handle_CurrencyNotFound_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),
                Type = "Hotel",
                Amount = 100,
                Currency = "$",  // Different currency
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = Guid.NewGuid(),
                    Symbol = "€"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync("€", It.IsAny<CancellationToken>())).ReturnsAsync(user.Currency);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync("$", It.IsAny<CancellationToken>())).ReturnsAsync((Currency)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.CURRENCY_NOT_FOUND, exception.Message);
        }

        [Fact]
        public async Task Handle_CurrencyMismatch_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),
                Type = "Hotel",
                Amount = 100,
                Currency = "$",  // Different currency
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = Guid.NewGuid(),
                    Symbol = "€"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync("€", It.IsAny<CancellationToken>())).ReturnsAsync(user.Currency);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync("$", It.IsAny<CancellationToken>())).ReturnsAsync(new Currency() { Id = Guid.NewGuid(), Symbol = "$" });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.CURRENCY_DOES_NOT_MATCH, exception.Message);
        }

        [Fact]
        public async Task Handle_ExpenseAlreadyExists_ShouldThrowFunctionalException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var command = new CreateExpenseCommand
            {
                UserId = userId,
                Date = DateTime.UtcNow.AddDays(-1),
                Type = "Restaurant",
                Amount = 100,
                Currency = "$",
                Comment = "Lunch"
            };

            var user = new User
            {
                Id = userId,
                CurrencyId = currencyId,
                Currency = new Currency()
                {
                    Id = currencyId,
                    Symbol = "$"
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _expenseRepositoryMock.Setup(r => r.ExpenseExistsAsync(It.IsAny<ExpenseExistsSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _currencyRepositoryMock.Setup(r => r.GetBySymbolAsync(command.Currency, It.IsAny<CancellationToken>())).ReturnsAsync(user.Currency);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FunctionalException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(FunctionalError.EXPENSE_ALREADY_EXIST, exception.Message);
        }
    }
}
