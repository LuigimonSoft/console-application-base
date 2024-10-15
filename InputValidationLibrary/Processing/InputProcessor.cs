using System;
using InputValidationLibrary.Mapping;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.ErrorMessages;
using InputValidationLibrary.Validation.Interfaces;

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
            // Step 1: Pre-validate the input parameters
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

                // Perform basic validations before mapping
                if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(parameterValue))
                {
                    validationResult.AddError(new Error()
                    {
                        ErrorCode = 0,
                        ErrorMessage = $"The value for '{prop.Name}' must not be empty or whitespace."
                    });
                }
                else if (prop.PropertyType == typeof(int))
                {
                    if (!int.TryParse(parameterValue, out _))
                    {
                        validationResult.AddError(new Error()
                        {
                            ErrorCode = 0,
                            ErrorMessage = $"The value for '{prop.Name}' must be a valid integer."
                        });
                    }
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    if (!decimal.TryParse(parameterValue, out _))
                    {
                        validationResult.AddError(new Error()
                        {
                            ErrorCode = 0,
                            ErrorMessage = $"The value for '{prop.Name}' must be a valid decimal."
                        });
                    }
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    if (!DateTime.TryParse(parameterValue, out _))
                    {
                        validationResult.AddError(new Error()
                        {
                            ErrorCode = 0,
                            ErrorMessage = $"The value for '{prop.Name}' must be a valid date."
                        });
                    }
                }
            }

            return validationResult;
        }
    }
}