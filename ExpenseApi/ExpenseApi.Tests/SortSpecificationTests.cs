using ExpenseApi.Model;
using ExpenseApi.Specification;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Assert = Xunit.Assert;

namespace ExpenseApi.Tests
{
    public class SortSpecificationTests
    {
        [Fact]
        public void Apply_SortsAscending_WhenAscendingIsTrue()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Amount = 100 },
                new Expense { Id = Guid.NewGuid(), Amount = 5.48 },
                new Expense { Id = Guid.NewGuid(), Amount = 200},
                new Expense { Id = Guid.NewGuid(), Amount = 200},
                new Expense { Id = Guid.NewGuid(), Amount = 30},
                new Expense { Id = Guid.NewGuid(), Amount = 146.78 },
                new Expense { Id = Guid.NewGuid(), Amount = 1406.78 }
            }.AsQueryable();

            var sortBy = (Expression<Func<Expense, double>>)(e => e.Amount);
            var specification = new SortSpecification<Expense, double>(sortBy, ascending: true);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert
            Assert.Equal(5.48, result.First().Amount);   // Ensures the second item is sorted ascending by Amount
            Assert.Equal(1406.78, result.Last().Amount);  // Ensures the first item is sorted ascending by Amount
        }

        [Fact]
        public void Apply_SortsDescending_WhenAscendingIsFalse()
        {
            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Amount = 100 },
                new Expense { Id = Guid.NewGuid(), Amount = 5.48 },
                new Expense { Id = Guid.NewGuid(), Amount = 200},
                new Expense { Id = Guid.NewGuid(), Amount = 200},
                new Expense { Id = Guid.NewGuid(), Amount = 146.78 },
                new Expense { Id = Guid.NewGuid(), Amount = 1406.78 }
            }.AsQueryable();

            var sortBy = (Expression<Func<Expense, double>>)(e => e.Amount);
            var specification = new SortSpecification<Expense, double>(sortBy, ascending: false);

            // Act
            var result = specification.Apply(expenses).ToList();

            // Assert
            Assert.Equal(1406.78, result.First().Amount);   // Ensures the second item is sorted descending by Amount
            Assert.Equal(5.48, result.Last().Amount);  // Ensures the first item is sorted descending by Amount
        }

        [Fact]
        public void Apply_SortsByMultipleColumns_WhenMultipleSortExpressionsAreUsed()
        {
            var firstDate = DateTime.UtcNow.AddDays(1);
            var secondDate = DateTime.UtcNow;
            var thirdDate = DateTime.UtcNow.AddDays(-1);

            // Arrange
            var expenses = new[]
            {
                new Expense { Id = Guid.NewGuid(), Amount = 100, Date = secondDate },
                new Expense { Id = Guid.NewGuid(), Amount = 5.48, Date = thirdDate },
                new Expense { Id = Guid.NewGuid(), Amount = 200, Date = firstDate },
                new Expense { Id = Guid.NewGuid(), Amount = 200, Date = firstDate },
                new Expense { Id = Guid.NewGuid(), Amount = 146.78, Date = thirdDate },
                new Expense { Id = Guid.NewGuid(), Amount = 1406.78, Date = thirdDate }
            }.AsQueryable();

            var sortByAmount = (Expression<Func<Expense, double>>)(e => e.Amount);
            var sortByDate = (Expression<Func<Expense, DateTime>>)(e => e.Date);
            var specificationAmount = new SortSpecification<Expense, double>(sortByAmount, ascending: true);
            var specificationDate = new SortSpecification<Expense, DateTime>(sortByDate, ascending: false);

            // Act
            var resultAmount = specificationAmount.Apply(expenses).ToList();
            var resultDate = specificationDate.Apply(expenses).ToList();

            // Assert: Verify sorting by Amount first
            Assert.Equal(5.48, resultAmount.First().Amount);
            Assert.Equal(1406.78, resultAmount.Last().Amount);

            // Assert: Verify sorting by Amount first
            Assert.Equal(firstDate, resultDate.First().Date);
            Assert.Equal(thirdDate, resultDate.Last().Date);
        }
    }
}
