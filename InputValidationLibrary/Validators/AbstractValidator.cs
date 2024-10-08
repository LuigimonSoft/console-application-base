using System;
using System.Collections.Generic;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        protected readonly List<Func<T, ValidationResult>> _rules = new List<Func<T, ValidationResult>>();

        public ValidationResult Validate(T instance)
        {
            var result = new ValidationResult();

            foreach (var rule in _rules)
            {
                var ruleResult = rule.Invoke(instance);
                if (!ruleResult.IsValid)
                {
                    result.IsValid = false;
                    result.Errors.AddRange(ruleResult.Errors);
                }
            }

            return result;
        }

        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> property)
        {
            return new RuleBuilder<T, TProperty>(this, property);
        }
    }
}

