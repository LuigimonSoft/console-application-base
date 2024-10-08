using System;
using InputValidationLibrary.Mapping;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.Interfaces;

namespace InputValidationLibrary.Processing
{
    public class InputProcessor<T> where T : new()
    {
        private readonly IValidator<T> _validator;

        public InputProcessor(IValidator<T> validator)
        {
            _validator = validator;
        }

        public ValidationResult Process(string[] parameters)
        {
            try
            {
                T model = ObjectMapper.MapFromParameters<T>(parameters);
                ValidationResult validationResult = _validator.Validate(model);
                return validationResult;
            }
            catch (Exception ex)
            {
                var result = new ValidationResult();
                result.AddError($"Mapping error: {ex.Message}");
                return result;
            }
        }
    }
}

