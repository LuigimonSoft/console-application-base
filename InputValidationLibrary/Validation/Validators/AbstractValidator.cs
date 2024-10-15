using System;
using System.Collections.Generic;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Validation.Validators
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        protected readonly List<IValidationRule<T>> _rules = new List<IValidationRule<T>>();
        private readonly Dictionary<string, List<IValidationRule<T>>> _propertyRules = new Dictionary<string, List<IValidationRule<T>>>();
        public ValidationResult Validate(T instance)
        {
            var result = new ValidationResult();

            foreach (var rule in _rules)
            {
                rule.Validate(instance, result);
            }

            return result;
        }

        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> property, string propertyName)
        {
            return new RuleBuilder<T, TProperty>(this, property, propertyName);
        }

        public void AddRule(IValidationRule<T> rule, string propertyName)
        {
            _rules.Add(rule);
            if (!_propertyRules.ContainsKey(propertyName))
            {
                _propertyRules[propertyName] = new List<IValidationRule<T>>();
            }
            _propertyRules[propertyName].Add(rule);
        }

        public IValidationRule<T> GetLastRule()
        {
            if (_rules.Count == 0)
            {
                throw new InvalidOperationException("No rules have been added yet.");
            }
            return _rules[^1];
        }

        public IEnumerable<IValidationRule<T>> GetRulesForProperty(string propertyName)
        {
            return _propertyRules.ContainsKey(propertyName) ? _propertyRules[propertyName] : new List<IValidationRule<T>>();
        }
    }
}
