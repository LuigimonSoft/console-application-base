using InputValidationLibrary.Validation;
using InputValidationLibrary.Tests.Models;
using InputValidationLibrary.Validation.Validators;

namespace InputValidationLibrary.Tests.Validation
{
    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithErrorCode(1001)
                .NotEmpty().WithErrorCode(1002);

            RuleFor(x => x.Age)
                .IsNumeric().WithErrorCode(1010)
                 .NotEmpty().WithErrorCode(1003)
                .InRange(18, 65).WithErrorCode(1004);

            RuleFor(x => x.Salary)
                .NotNull().WithErrorCode(1005)
                .IsDecimal().WithErrorCode(1006);

            RuleFor(x => x.FilePath)
                .NotNull().WithErrorCode(1007)
                .NotEmpty().WithErrorCode(1008)
                .IsPath().WithErrorCode(1009);
        }
    }
}