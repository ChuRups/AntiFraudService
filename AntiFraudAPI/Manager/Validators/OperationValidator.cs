using Domain;
using Domain.Enums;
using FluentValidation;
using Interfaces.Repositories;
using Interfaces.Validators;

namespace Manager.Validators
{
    public class OperationValidator : AbstractValidator<Operation>, ICustomValidator<Operation>
    {
        private readonly IOperationRepository _operationRepository;
        private ActionType _actionType;
        private const int MAX_TRANSACTION_AMOUNT = 2000;
        private const int MAX_DAILY_AMOUNT = 20000;
        public List<string> Errors { get; private set; }

        public OperationValidator(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
            Validations();
        }

        public async Task<bool> IsValidAsync(Operation operation, ActionType actionType)
        {
            _actionType = actionType;

            var validationResult = await base.ValidateAsync(operation);
            Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return validationResult.IsValid;
        }

        public void Validations()
        {
            RuleFor(x => x.IdCustomer)
               .NotEmpty().WithMessage("Customer is required.")
               .When(x => _actionType == ActionType.Create);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .Must(x => x <= MAX_TRANSACTION_AMOUNT)
                .WithErrorCode("HasExceededMaximumAmountPerTransferError")
                .WithMessage("HasExceededMaximumAmountPerTransferError")
                .When(x => _actionType == ActionType.Create);


            RuleFor(x => x)
                .MustAsync(async (x, cancellation) =>
                {
                    return await HasExceededMaximumAmountPerTransfer(x);
                })
                .WithErrorCode("HasExceededMaximumDailyAmountError")
                .WithMessage("HasExceededMaximumDailyAmountError")
                .When(x => _actionType == ActionType.Create);
        }

        #region [PrivateMethods]
        private async Task<bool> HasExceededMaximumAmountPerTransfer(Operation operation)
        {
            var dailyTotal = await _operationRepository.GetOperationsSumAsync(operation);
            return (dailyTotal + operation.Amount) <= MAX_DAILY_AMOUNT;
        }

        #endregion
    }
}
