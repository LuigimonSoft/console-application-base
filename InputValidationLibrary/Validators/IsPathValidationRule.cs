using System;
using System.IO;
using InputValidationLibrary.Validation.interfaces;
using InputValidationLibrary.Validation.ErrorMessages;

namespace InputValidationLibrary.Validation.Validators
{
  public class IsPathValidationRule<T> : IValidationRule<T>
  {
    private readonly Func<T, object> _property;
    public int? ErrorCode { get; set; }

    public IsPathValidationRule(Func<T, object> property)
    {
      _property = property;
    }

    public void Validate(T entity, ValidationResult result)
    {
      var value = _property(entity);
      if (!string.IsNullOrEmpty(value))
      {
        try
        {
          var isValidPath = Path.IsPathRooted(value) && !Path.GetInvalidPathChars().IsAnyInvalidCharacterInPath(value);
          if (!isValidPath)
          {
            if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var erroMessage))
            {
              result.AddError(erroMessage);
            }
            else
            {
              result.AddError("the value must be a valid path.");
            }
          }

        }
        catch (Exception)
        {
          result.AddError("the value must be a valid path.");
        }
      }
    }
  }
}