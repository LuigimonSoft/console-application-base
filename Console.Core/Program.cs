using System;
using ConsoleBase.Validators;
using InputValidationLibrary.Processing;
using InputValidationLibrary.Validators;

namespace ConsoleBase.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
             var validator = new BaseModelValidator();

             var ProcessValidation = new InputProcessor<BaseModel>(validator, ErrorMessages.ErrorList);

             var ValidationResult = ProcessValidation.Process(args);

            if (ValidationResult.IsValid)
                Console.WriteLine("Validation passed.");
            else
            {
                Console.WriteLine("Validation failed.");
                foreach (var error in ValidationResult.Errors)
                    Console.WriteLine($"Error code: {error.ErrorCode}, Error message: {error.ErrorMessage}");

            }
        }
    }
}
