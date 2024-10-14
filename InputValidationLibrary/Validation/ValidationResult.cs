using System.Collections.Generic;

namespace InputValidationLibrary.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public List<Error> Errors { get; } = new List<Error>();

        public void AddError(Error error)
        {
            IsValid = false;
            Errors.Add(error);
        }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}

