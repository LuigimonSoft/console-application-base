using System;
using System.Collections.Generic;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        protected readonly List<IValidationRule<T>> _rules = new List<IValidationRule<T>>();

        public ValidationResult Validate(T instance)
        {
            var result = new ValidationResult();

            foreach (var rule in _rules)
            {
                rule.Validate(instance, result);
            }

            return result;
        }

        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> property)
        {
            return new RuleBuilder<T, TProperty>(this, property);
        }

        public void AddRule(IValidationRule<T> rule)
        {
            _rules.Add(rule);
        }

        public IValidationRule<T> GetLastRule()
        {
            if (_rules.Count == 0)
            {
                throw new InvalidOperationException("No rules have been added yet.");
            }
            return _rules[^1];
        }
    }
}
