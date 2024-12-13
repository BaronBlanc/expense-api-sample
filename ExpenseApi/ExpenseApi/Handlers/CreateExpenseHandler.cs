using AutoMapper;
using ExpenseApi.Commands;
using ExpenseApi.Exceptions;
using ExpenseApi.Model;
using ExpenseApi.Repository;
using ExpenseApi.Specification;
using MediatR;
using System.Formats.Asn1;

namespace ExpenseApi.Handlers
{
    public class CreateExpenseHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateExpenseCommand, CreateExpenseResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CreateExpenseResult> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken) ?? throw new FunctionalException(FunctionalError.USER_NOT_FOUND, StatusCodes.Status404NotFound);
            if (request.Date > DateTime.UtcNow)
                throw new FunctionalException(FunctionalError.EXPENSE_DATE_CANNOT_BE_IN_FUTURE, StatusCodes.Status400BadRequest);

            if (request.Date < DateTime.UtcNow.AddMonths(-3))
                throw new FunctionalException(FunctionalError.EXPENSE_DATE_CANNOT_BE_TOO_OLD, StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(request.Comment))
                throw new FunctionalException(FunctionalError.COMMENT_REQUIRED, StatusCodes.Status400BadRequest);

            var currency = await _unitOfWork.Currencies.GetBySymbolAsync(request.Currency, cancellationToken) ?? throw new FunctionalException(FunctionalError.CURRENCY_NOT_FOUND, StatusCodes.Status404NotFound);
            
            if (currency.Id != user.CurrencyId)
                throw new FunctionalException(FunctionalError.CURRENCY_DOES_NOT_MATCH, StatusCodes.Status400BadRequest);

            var specification = new ExpenseExistsSpecification(request.UserId, request.Date, request.Amount);
            var expenseExists = await _unitOfWork.Expenses.ExpenseExistsAsync(specification, cancellationToken);

            if (expenseExists)
                throw new FunctionalException(FunctionalError.EXPENSE_ALREADY_EXIST, StatusCodes.Status400BadRequest);

            Expense expenseToSave;
            try
            {
                expenseToSave = _mapper.Map<Expense>(request);
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                // Yes it's not beautyfull but you don't have the choice with automapper 
                // https://stackoverflow.com/questions/22409895/automapper-handling-custom-made-exceptions-so-cant-handle-them-elsewhere
                throw new FunctionalException(autoMapperException.InnerException.Message, StatusCodes.Status400BadRequest);
            }

            expenseToSave.CurrencyId = currency.Id;

            await _unitOfWork.Expenses.AddAsync(expenseToSave, cancellationToken);

            await _unitOfWork.Expenses.SaveAsync();

            return _mapper.Map<CreateExpenseResult>(expenseToSave);
        }
    }
}
