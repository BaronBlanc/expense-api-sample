using AutoMapper;
using ExpenseApi.Commands;
using ExpenseApi.Exceptions;
using ExpenseApi.Model;
using ExpenseApi.Repository;
using ExpenseApi.Specification;
using MediatR;

namespace ExpenseApi.Handlers
{
    public class GetExpensesHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetExpensesQuery, List<ExpenseView>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ExpenseView>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken) ?? throw new FunctionalException(FunctionalError.USER_NOT_FOUND, StatusCodes.Status404NotFound);
            
            ISpecification<Expense> specification;
            if (request.SortBy == ESortType.Date) {
                specification = new ExpenseDateTimeSortSpecification(request.Ascending);
            }
            else
            {
                specification = new ExpenseAmountSortSpecification(request.Ascending);
            }
            var expenseResults = await _unitOfWork.Expenses.GetAllByUserIdSortedAsync(user.Id, specification, cancellationToken);

            return _mapper.Map<List<ExpenseView>>(expenseResults);
        }
    }

}
