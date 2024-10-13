using System;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public class RuleBuilder<T, TProperty>
    {
        private readonly AbstractValidator<T> _validator;
        private readonly Func<T, TProperty> _property;

        public RuleBuilder(AbstractValidator<T> validator, Func<T, TProperty> property)
        {
            _validator = validator;
            _property = property;
        }

        public RuleBuilder<T, TProperty> NotNull()
        {
            var rule = new NotNullValidationRule<T, TProperty>(_property);
            _validator.AddRule(rule);
            return this;
        }

        public RuleBuilder<T, TProperty> NotEmpty()
        {
            var rule = new NotEmptyValidationRule<T, TProperty>(_property);
            _validator.AddRule(rule);
            return this;
        }

        public RuleBuilder<T, TProperty> InRange(TProperty min, TProperty max)
        {
            var rule = new RangeValidationRule<T, TProperty>(_property, min, max);
            _validator.AddRule(rule);
            return this;
        }

        public RuleBuilder<T, TProperty> IsDecimal()
        {
            var rule = new IsDecimalValidationRule<T>(_property as Func<T, object>);
            _validator.AddRule(rule);
            return this;
        }

        public RuleBuilder<T, TProperty> Length(int minLength, int maxLength)
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new LengthValidationRule<T>(stringProperty, minLength, maxLength);
                _validator.AddRule(rule);
            }
            else
            {
                throw new InvalidOperationException("Length validation can only be applied to string properties.");
            }
            return this;
        }

        public RuleBuilder<T, TProperty> IsNumeric()
        {
            var rule = new IsNumericValidationRule<T>(_property as Func<T, object>);
            _validator.AddRule(rule);
            return this;
        }

        public RuleBuilder<T, TProperty> WithErrorCode(int errorCode)
        {
            var lastRule = _validator.GetLastRule();
            lastRule.ErrorCode = errorCode;
            return this;
        }
    }
}


