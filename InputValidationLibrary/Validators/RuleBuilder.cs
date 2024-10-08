using System;
using InputValidationLibrary.Validation;

namespace InputValidationLibrary.Validation.Validators
{
    public class RuleBuilder<T, TProperty>
    {
        private readonly AbstractValidator<T> _validator;
        private readonly Func<T, TProperty> _property;
        private Func<T, ValidationResult> _currentRule;

        public RuleBuilder(AbstractValidator<T> validator, Func<T, TProperty> property)
        {
            _validator = validator;
            _property = property;
        }

        public RuleBuilder<T, TProperty> NotNull()
        {
            _currentRule = instance =>
            {
                var value = _property(instance);
                var result = new ValidationResult();
                if (value == null)
                {
                    result.AddError("The value cannot be null.");
                }
                return result;
            };
            _validator._rules.Add(_currentRule);
            return this;
        }

        public RuleBuilder<T, TProperty> NotEmpty()
        {
            _currentRule = instance =>
            {
                var value = _property(instance) as string;
                var result = new ValidationResult();
                if (string.IsNullOrWhiteSpace(value))
                {
                    result.AddError("The value cannot be empty.");
                }
                return result;
            };
            _validator._rules.Add(_currentRule);
            return this;
        }

        public RuleBuilder<T, TProperty> InRange<TValue>(TValue min, TValue max) where TValue : IComparable
        {
            _currentRule = instance =>
            {
                var value = _property(instance);
                var result = new ValidationResult();

                if (value is IComparable comparableValue)
                {
                    if (comparableValue.CompareTo(min) < 0 || comparableValue.CompareTo(max) > 0)
                    {
                        result.AddError($"The value must be between {min} and {max}.");
                    }
                }
                else
                {
                    result.AddError("The value must be a numeric type.");
                }

                return result;
            };
            _validator._rules.Add(_currentRule);
            return this;
        }

        public RuleBuilder<T, TProperty> WithErrorCode(int errorCode)
        {
            var lastRule = _validator._rules[_validator._rules.Count - 1];
            _validator._rules[_validator._rules.Count - 1] = instance =>
            {
                var result = lastRule.Invoke(instance);
                if (!result.IsValid)
                {
                    result.Errors[0] = $"{result.Errors[0]} (Error Code: {errorCode})";
                }
                return result;
            };
            return this;
        }
    }
}

