using InputValidationLibrary.Validation;

namespace InputValidationLibrary.Test.Validation
{
    public class TestModelValidator : Validator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithErrorCode(1001)
                .NotEmpty().WithErrorCode(1002);

            RuleFor(x => x.Age)
                .InRange(18, 65).WithErrorCode(1003);

            RuleFor(x => x.Salary)
                .NotNull().WithErrorCode(1004)
                .IsDecimal().WithErrorCode(1005);

            RuleFor(x => x.FilePath)
                .NotNull().WithErrorCode(1006)
                .NotEmpty().WithErrorCode(1007)
                .IsPath().WithErrorCode(1008);
        }
    }
}