using ExpenseApi.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ExpenseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // POST: api/Expenses
        // POST: api/Expenses
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand expense, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(expense, cancellationToken);
                return result switch
                {
                    null => BadRequest("Expense could not be created."),
                    var expenseToSave => Ok(expenseToSave)
                };

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Expenses
        [HttpGet]
        public async Task<IActionResult> GetExpenses(CancellationToken cancellationToken, [FromQuery] Guid userId, [FromQuery] bool ascending, [FromQuery] string sortBy = "Date")
        {
            try
            {
                var query = new GetExpensesQuery(userId, ascending, sortBy);
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
