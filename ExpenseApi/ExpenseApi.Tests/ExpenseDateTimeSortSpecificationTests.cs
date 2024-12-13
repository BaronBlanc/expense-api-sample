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
    public class ExpenseDateTimeSortSpecificationTests
    {
        [Fact]
        public void Apply_SortsByDateTimeDescending_WhenSortByIsDateTimeAndAscendingIsFalse()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(2) }
            }.AsQueryable();

            var specification = new ExpenseDateTimeSortSpecification(false);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert: Ensure the expenses are sorted by DateTime in descending order
            Assert.Equal(expenses.OrderByDescending(e => e.Date).First().Date, result.First().Date);
            Assert.Equal(expenses.OrderByDescending(e => e.Date).Last().Date, result.Last().Date);
        }

        [Fact]
        public void Apply_SortsByDateTimeAscending_WhenSortByDateTimeAndAscendingIsTrue()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(-1) },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(1) },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(-1) },
                new Expense { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(-1) }
            }.AsQueryable();

            var specification = new ExpenseDateTimeSortSpecification(true);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert: Ensure the expenses are sorted by DateTime in ascending order
            Assert.Equal(expenses.OrderBy(e => e.Date).First().Date, result.First().Date);
            Assert.Equal(expenses.OrderBy(e => e.Date).Last().Date, result.Last().Date);
        }
    }
}
