using ExpenseApi.Model;
using ExpenseApi.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpenseApi.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<DbSet<Expense>> _mockExpenseSet;
        private readonly Mock<ExpenseDbContext> _mockContext;
        private readonly Repository<Expense> _repository;

        public RepositoryTests()
        {
            _mockExpenseSet = new Mock<DbSet<Expense>>();
            _mockContext = new Mock<ExpenseDbContext>();

            // Set up the mock context to return the mocked DbSet<Expense>
            _mockContext.Setup(c => c.Set<Expense>()).Returns(_mockExpenseSet.Object);

            _repository = new Repository<Expense>(_mockContext.Object);
        }

        // Test GetByIdAsync Method
        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenEntityExists()
        {
            // Arrange
            var testExpense = new Expense { Id = Guid.NewGuid(), Amount = 100, Date = DateTime.UtcNow, UserId = Guid.NewGuid() };
            _mockExpenseSet
                .Setup(m => m.FindAsync(new object[] { testExpense.Id }, CancellationToken.None))
                .ReturnsAsync(testExpense);

            // Act
            var result = await _repository.GetByIdAsync(testExpense.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testExpense.Id, result.Id);
            _mockExpenseSet.Verify(m => m.FindAsync(new object[] { testExpense.Id }, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenEntityDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockExpenseSet.Setup(m => m.FindAsync(nonExistentId, CancellationToken.None))
                           .ReturnsAsync((Expense)null);

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockExpenseSet.Verify(m => m.FindAsync(new object[] { nonExistentId }, CancellationToken.None), Times.Once);
        }

        // Test AddAsync Method
        [Fact]
        public async Task AddAsync_AddsEntityToDbSet()
        {
            // Arrange
            var newExpense = new Expense { Id = Guid.NewGuid(), Amount = 100, Date = DateTime.UtcNow, UserId = Guid.NewGuid() };

            // Act
            await _repository.AddAsync(newExpense, CancellationToken.None);

            // Assert
            _mockExpenseSet.Verify(m => m.AddAsync(newExpense, CancellationToken.None), Times.Once);
        }

        // Test SaveAsync Method
        [Fact]
        public async Task SaveAsync_CommitsChangesToDatabase()
        {
            // Arrange
            var newExpense = new Expense { Id = Guid.NewGuid(), Amount = 200, Date = DateTime.UtcNow, UserId = Guid.NewGuid() };
            _mockExpenseSet.Setup(m => m.AddAsync(newExpense, CancellationToken.None)).Verifiable();

            // Act
            await _repository.AddAsync(newExpense, CancellationToken.None);
            await _repository.SaveAsync();

            // Assert
            _mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}
