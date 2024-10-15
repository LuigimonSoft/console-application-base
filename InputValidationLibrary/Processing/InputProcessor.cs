using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InputValidationLibrary.Mapping;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.ErrorMessages;
using InputValidationLibrary.Validation.Interfaces;
using ConsoleBase.Common.Attributes;
using InputValidationLibrary.Validation.Validators;

namespace InputValidationLibrary.Processing
{
    public class InputProcessor<T> where T : new()
    {
        private readonly IValidator<T> _validator;

        public InputProcessor(IValidator<T> validator, Dictionary<int, string> errorMessages)
        {
            _validator = validator;
            ErrorMessageStore.Messages = errorMessages;
        }

        public ValidationResult Process(string[] parameters)
        {
            var preValidationResult = PreValidateInputs(parameters);
            if (!preValidationResult.IsValid)
            {
                return preValidationResult;
            }

            try
            {
                T model = ObjectMapper.MapFromParameters<T>(parameters);

                return _validator.Validate(model);
            }
            catch (Exception ex)
            {
                var result = new ValidationResult();
                result.AddError(new Error() { ErrorCode = 0, ErrorMessage = $"Error when mapping: {ex.Message}" });
                return result;
            }
        }

        private ValidationResult PreValidateInputs(string[] parameters)
        {
            var validationResult = new ValidationResult();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;

                var position = columnAttr.Position;

                if (position >= parameters.Length)
                {
                    validationResult.AddError(new Error()
                    {
                        ErrorCode = 0,
                        ErrorMessage = $"The input parameter at position {position} is missing for the property '{prop.Name}'."
                    });
                    continue;
                }

                var parameterValue = parameters[position];

                var rules = (_validator as AbstractValidator<T>).GetRulesForPosition(position);

                foreach (var rule in rules)
                {
                    rule.ValidateValue(parameterValue, validationResult);
                }
            }

            return validationResult;
        }
    }
}
