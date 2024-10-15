using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InputValidationLibrary.Mapping;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.ErrorMessages;
using InputValidationLibrary.Validation.Interfaces;
using ConsoleBase.Common.Attributes;

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
            // Step 1: Pre-validate the input parameters before mapping
            var preValidationResult = PreValidateInputs(parameters);
            if (!preValidationResult.IsValid)
            {
                return preValidationResult;
            }

            try
            {
                // Step 2: Map the input parameters to the model
                T model = ObjectMapper.MapFromParameters<T>(parameters);

                // Step 3: Validate the mapped model
                return _validator.Validate(model);
            }
            catch (Exception ex)
            {
                var result = new ValidationResult();
                result.AddError(new Error() { ErrorCode = 0, ErrorMessage = $"Mapping error: {ex.Message}" });
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
                        ErrorMessage = $"Missing input parameter at position {position} for property '{prop.Name}'."
                    });
                    continue;
                }

                var parameterValue = parameters[position];

                // Extract the rules defined for the property
                var rules = _validator.GetRulesForProperty(prop.Name);

                foreach (var rule in rules)
                {
                    // Apply the rule based on its type
                    switch (rule)
                    {
                        case NotEmptyValidationRule<T> notEmptyRule:
                            if (string.IsNullOrWhiteSpace(parameterValue))
                            {
                                validationResult.AddError(new Error()
                                {
                                    ErrorCode = notEmptyRule.ErrorCode ?? 0,
                                    ErrorMessage = ErrorMessageStore.GetMessage(notEmptyRule.ErrorCode ?? 0)
                                });
                            }
                            break;
                        
                        case IsNumericValidationRule<T> isNumericRule:
                            if (!int.TryParse(parameterValue, out _))
                            {
                                validationResult.AddError(new Error()
                                {
                                    ErrorCode = isNumericRule.ErrorCode ?? 0,
                                    ErrorMessage = ErrorMessageStore.GetMessage(isNumericRule.ErrorCode ?? 0)
                                });
                            }
                            break;

                        case IsDecimalValidationRule<T> isDecimalRule:
                            if (!decimal.TryParse(parameterValue, out _))
                            {
                                validationResult.AddError(new Error()
                                {
                                    ErrorCode = isDecimalRule.ErrorCode ?? 0,
                                    ErrorMessage = ErrorMessageStore.GetMessage(isDecimalRule.ErrorCode ?? 0)
                                });
                            }
                            break;

                        // Add more cases if needed for other rules...
                    }

                    // If an error has already been added for this property, break out of the loop
                    if (!validationResult.IsValid)
                    {
                        break;
                    }
                }
            }

            return validationResult;
        }
    }
}