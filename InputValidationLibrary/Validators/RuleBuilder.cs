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

        public RuleBuilder<T, TProperty> InRange(TProperty min, TProperty max)
        {
            var rule = new RangeValidationRule<T, TProperty>(_property, min, max);
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


