using System;
using InputValidationLibrary.Validation.Interfaces;
using InputValidationLibrary.Validation.ErrorMessages;

namespace InputValidationLibrary.Validation.Validators
{
    public class NotNullValidationRule<T, TProperty> : IValidationRule<T>
    {
        private readonly Func<T, TProperty> _property;
        public int? ErrorCode { get; set; }

        public NotNullValidationRule(Func<T, TProperty> property)
        {
            _property = property;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);
            if (value == null)
            {
                if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var errorMessage))
                {
                    result.AddError(new Error() { ErrorCode.Value, erroMessage });
                }
                else
                {
                    result.AddError(new Error() { ErrorCode.Value, "The value cannot be null." });
                }
            }
        }
    }
}

