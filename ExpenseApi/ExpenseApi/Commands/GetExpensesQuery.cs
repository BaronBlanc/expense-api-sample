using ExpenseApi.Exceptions;
using ExpenseApi.Model;
using MediatR;
using System.Globalization;

namespace ExpenseApi.Commands
{
    public enum ESortType
    {
        Date,
        Amount
    }

    public class GetExpensesQuery : IRequest<List<ExpenseView>>
    {
        public Guid UserId { get; set;  }
        public bool Ascending { get; set; }
        public ESortType SortBy { get; set; }

        public GetExpensesQuery(Guid userId, bool ascending, string sortBy)
        {
            if (!Enum.TryParse<ESortType>(sortBy, true, out var sortByStruct))
            {
                throw new FunctionalException(FunctionalError.SORT_EXPRESSION_NOT_VALID, StatusCodes.Status400BadRequest);
            }

            UserId = userId;
            Ascending = ascending;
            SortBy = sortByStruct;
        }
    }

}
