using System;
using InputValidationLibrary.Validation.Interfaces;
using InputValidationLibrary.Validation.ErrorMessages;

namespace InputValidationLibrary.Validation.Validators
{
  public class LengthValidationRule<T> : IValidationRule<T>
  {
    private readonly Func<T, string> _property;
    private readonly int _minLength;
    private readonly int _maxLength;
    public int? ErrorCode { get; set; }

    public LengthValidationRule(Func<T, string> property, int minLength, int maxLength)
    {
      _property = property;
      _minLength = minLength;
      _maxLength = maxLength;
    }

    public void Validate(T entity, ValidationResult result)
    {
      var value = _property(entity);
      if (value != null)
      {
        if (value.Length < _minLength || value.Length > _maxLength)
        {
          if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var erroMessage))
          {
            result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = erroMessage });
          }
          else
          {
            result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = $"The value length must be between {_minLength} and {_maxLength} characters." });
          }
        }
      }
    }
  }
}