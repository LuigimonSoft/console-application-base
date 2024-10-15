using System;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public class RuleBuilder<T, TProperty>
    {
        private readonly AbstractValidator<T> _validator;
        private readonly Func<T, TProperty> _property;
        private readonly string _propertyName;

        public RuleBuilder(AbstractValidator<T> validator, Func<T, TProperty> property, string propertyName)
        {
            _validator = validator;
            _property = property;
            _propertyName = propertyName;
        }

        public RuleBuilder<T, TProperty> NotNull()
        {
            var rule = new NotNullValidationRule<T, TProperty>(_property);
            _validator.AddRule(rule, _propertyName);
            return this;
        }

        public RuleBuilder<T, TProperty> NotEmpty()
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new NotEmptyValidationRule<T>(stringProperty);
                _validator.AddRule(rule, _propertyName);
            }
            else
            {
                throw new InvalidOperationException("NotEmpty can only be used with string properties.");
            }

            return this;
        }

        public RuleBuilder<T, TProperty> IsPath()
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new IsPathValidationRule<T>(stringProperty);
                _validator.AddRule(rule, _propertyName);
            }
            else
            {
                throw new InvalidOperationException("IsPath can only be used with string properties.");
            }

            return this;
        }


        public RuleBuilder<T, TProperty> InRange(TProperty min, TProperty max)
        {
            var rule = new RangeValidationRule<T, TProperty>(_property, min, max);
            _validator.AddRule(rule, _propertyName);
            return this;
        }

        public RuleBuilder<T, TProperty> IsDecimal()
        {
            var rule = new IsDecimalValidationRule<T>(_property as Func<T, object>);
            _validator.AddRule(rule, _propertyName);
            return this;
        }

        public RuleBuilder<T, TProperty> Length(int minLength, int maxLength)
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new LengthValidationRule<T>(stringProperty, minLength, maxLength);
                _validator.AddRule(rule, _propertyName);
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
            _validator.AddRule(rule, _propertyName);
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


