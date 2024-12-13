using ExpenseApi.Commands;
using ExpenseApi.Model;
using ExpenseApi.Specification;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Assert = Xunit.Assert;

namespace ExpenseApi.Tests
{
    public class ExpenseAmountSortSpecificationTests
    {
        [Fact]
        public void Apply_SortsByAmountDescending_WhenSortByIsAmountAndAscendingIsFalse()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Amount = 100, Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Amount = 200, Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Amount = 150, Date = DateTime.UtcNow.AddDays(2) }
            }.AsQueryable();

            var specification = new ExpenseAmountSortSpecification(false);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert: Ensure the expenses are sorted by Amount in descending order
            Assert.Equal(expenses.OrderByDescending(e => e.Amount).First().Amount, result.First().Amount);
            Assert.Equal(expenses.OrderByDescending(e => e.Amount).Last().Amount, result.Last().Amount);
        }

        [Fact]
        public void Apply_SortsByAmountAscending_WhenSortByAmountAndAscendingIsTrue()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Amount = 100, Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Amount = 1406.50, Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Amount = 5.60, Date = DateTime.UtcNow.AddDays(-1) },
                new Expense { Id = Guid.NewGuid(), Amount = 50, Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Amount = 50.5, Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Amount = 90, Date = DateTime.UtcNow.AddDays(-1) },
                new Expense { Id = Guid.NewGuid(), Amount = 200, Date = DateTime.UtcNow.AddDays(-1) }
            }.AsQueryable();

            var specification = new ExpenseAmountSortSpecification(true);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert: Ensure the expenses are sorted by Amount in ascending order
            Assert.Equal(expenses.OrderBy(e => e.Amount).First().Amount, result.First().Amount);
            Assert.Equal(expenses.OrderBy(e => e.Amount).Last().Amount, result.Last().Amount);
        }
    }
}
