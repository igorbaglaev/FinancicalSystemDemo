using FinancialSystem.Web.Models.Request;
using FluentValidation;

namespace FinancialSystem.Web.Validation
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(e => e.Amount).GreaterThan(0).WithMessage("Positive value required");
            RuleFor(e => e.Type).IsInEnum();
        }
    }
}
