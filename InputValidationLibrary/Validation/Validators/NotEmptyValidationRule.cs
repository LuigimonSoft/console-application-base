using System;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public class NotEmptyValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, string> _property;
        public int? ErrorCode { get; set; }

        public NotEmptyValidationRule(Func<T, string> property)
        {
            _property = property;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);

            if (string.IsNullOrWhiteSpace(value))
            {
                if (ErrorCode.HasValue && errorMessages.TryGetValue(ErrorCode.Value, out var errorMessage))
                {
                    result.AddError(errorMessage);
                }
                else
                {
                    result.AddError("The value must not be empty or consist solely of whitespace.");
                }
            }
        }
    }
}