namespace InputValidationLibrary.Validation.Interfaces
{
    public interface IValidationRule<T>
    {
        void Validate(T entity, ValidationResult result);
        int? ErrorCode { get; set; }
    }
}

