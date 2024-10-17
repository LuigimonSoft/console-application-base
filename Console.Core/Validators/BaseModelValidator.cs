using InputValidationLibrary.Validation;
using ConsoleBase.Core.Models;
using InputValidationLibrary.Validation.Validators;

namespace ConsoleBase.Validators
{
  public class BaseModelValidator : AbstractValidator<BaseModel>
  {
    public BaseModelValidator()
    {
      RuleFor(x => x.Name)
          .NotNull().WithErrorCode(1001)
          .NotEmpty().WithErrorCode(1002);

      RuleFor(x => x.Age)
          .IsNumeric().WithErrorCode(1010)
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