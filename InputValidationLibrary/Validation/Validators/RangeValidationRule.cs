using System;
using InputValidationLibrary.Validation.Interfaces;
using InputValidationLibrary.Validation.ErrorMessages;

namespace InputValidationLibrary.Validation.Validators
{
    public class RangeValidationRule<T, TProperty> : IValidationRule<T>
    {
        private readonly Func<T, TProperty> _property;
        private readonly TProperty _min;
        private readonly TProperty _max;
        public int? ErrorCode { get; set; }

        public RangeValidationRule(Func<T, TProperty> property, TProperty min, TProperty max)
        {
            _property = property;
            _min = min;
            _max = max;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);

            if (value is IComparable comparableValue && _min is IComparable minComparable && _max is IComparable maxComparable)
            {
                if (comparableValue.CompareTo(minComparable) < 0 || comparableValue.CompareTo(maxComparable) > 0)
                {
                    if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var errorMessage))
                    {
                        result.AddError(new Error() { ErrorCode=ErrorCode.Value, ErrorMessage = string.Format(errorMessage, _min, _max) });
                    }
                    else
                    {
                        result.AddError(new Error() { ErrorCode=ErrorCode.Value, ErrorMessage = $"The value must be between {_min} and {_max}." });
                    }
                }
            }
            else
            {
                result.AddError("The value must be comparable.");
            }
        }
    }
}
