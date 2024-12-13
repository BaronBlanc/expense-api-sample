using ExpenseApi.Model;
using ExpenseApi.Specification;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Assert = Xunit.Assert;

namespace ExpenseApi.Tests
{
    public class ExpenseExistsSpecificationTests
    {
        [Fact]
        public void ToExpression_ReturnsCorrectExpression_WhenValidParametersAreProvided()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var amount = 100.50;

            var specification = new ExpenseExistsSpecification(userId, date, amount);

            // Act
            var expression = specification.ToExpression();

            // Assert: Verify that the expression works as expected
            var expense = new Expense
            {
                UserId = userId,
                Date = date,
                Amount = amount
            };

            // Create the compiled expression
            var compiledExpression = expression.Compile();

            // Assert the expression matches the provided values
            Assert.True(compiledExpression(expense));
        }

        [Fact]
        public void ToExpression_ReturnsFalse_WhenUserIdDoesNotMatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var amount = 100.50;
            var invalidUserId = Guid.NewGuid();

            var specification = new ExpenseExistsSpecification(userId, date, amount);

            // Act
            var expression = specification.ToExpression();
            var compiledExpression = expression.Compile();

            // Assert: Verify that the expression returns false when UserId doesn't match
            var expense = new Expense
            {
                UserId = invalidUserId,
                Date = date,
                Amount = amount
            };

            Assert.False(compiledExpression(expense));
        }

        [Fact]
        public void ToExpression_ReturnsFalse_WhenDateDoesNotMatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var amount = 100.50;
            var invalidDate = DateTime.UtcNow.AddDays(1);

            var specification = new ExpenseExistsSpecification(userId, date, amount);

            // Act
            var expression = specification.ToExpression();
            var compiledExpression = expression.Compile();

            // Assert: Verify that the expression returns false when Date doesn't match
            var expense = new Expense
            {
                UserId = userId,
                Date = invalidDate,
                Amount = amount
            };

            Assert.False(compiledExpression(expense));
        }

        [Fact]
        public void ToExpression_ReturnsFalse_WhenAmountDoesNotMatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var amount = 100.50;
            var invalidAmount = 200.60;

            var specification = new ExpenseExistsSpecification(userId, date, amount);

            // Act
            var expression = specification.ToExpression();
            var compiledExpression = expression.Compile();

            // Assert: Verify that the expression returns false when Amount doesn't match
            var expense = new Expense
            {
                UserId = userId,
                Date = date,
                Amount = invalidAmount
            };

            Assert.False(compiledExpression(expense));
        }

        [Fact]
        public void ToExpression_ReturnsTrue_WhenAllFieldsMatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var amount = 100.50;

            var specification = new ExpenseExistsSpecification(userId, date, amount);

            // Act
            var expression = specification.ToExpression();
            var compiledExpression = expression.Compile();

            // Assert: Verify that the expression returns true when all fields match
            var expense = new Expense
            {
                UserId = userId,
                Date = date,
                Amount = amount
            };

            Assert.True(compiledExpression(expense));
        }
    }
}
