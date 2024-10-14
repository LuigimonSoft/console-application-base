using System;
using InputValidationLibrary.Validation.Interfaces;
using InputValidationLibrary.Validation.ErrorMessages;

namespace InputValidationLibrary.Validation.Validators
{
  public class IsNumericValidationRule<T> : IValidationRule<T>
  {
    private readonly Func<T, object> _property;
    public int? ErrorCode { get; set; }

    public IsNumericValidationRule(Func<T, object> property)
    {
      _property = property;
    }

    public void Validate(T entity, ValidationResult result)
    {
      var value = _property(entity)?.ToString();
      if (value != null && !int .TryParse(value, out _))
      {
        if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var erroMessage))
        {
          result.AddError(new Error() { ErrorCode.Value, erroMessage });
        }
        else
        {
          result.AddError(new Error() { ErrorCode.Value, "The value must be numeric." });
        }
        
      }
    }
  }
}