using System.Collections.Generic;

namespace InputValidationLibrary.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public List<string> Errors { get; } = new List<string>();

        public void AddError(string error)
        {
            IsValid = false;
            Errors.Add(error);
        }
    }
}

