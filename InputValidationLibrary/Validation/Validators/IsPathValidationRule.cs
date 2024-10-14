using System;
using System.IO;
using InputValidationLibrary.Validation.Interfaces;
using InputValidationLibrary.Validation.ErrorMessages;
using System.Runtime.InteropServices;

namespace InputValidationLibrary.Validation.Validators
{
  public class IsPathValidationRule<T> : IValidationRule<T>
  {
    private readonly Func<T, string> _property;
    public int? ErrorCode { get; set; }

    public IsPathValidationRule(Func<T, string> property)
    {
      _property = property;
    }

    public void Validate(T entity, ValidationResult result)
    {
      var value = _property(entity);
      if (!string.IsNullOrEmpty(value))
      {
        try
        {
          if (!IsValidPath(value))
          {
            if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var erroMessage))
            {
              rresult.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = erroMessage });
            }
            else
            {
              result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = "the value must be a valid path." });
            }
          }

        }
        catch (Exception)
        {
          result.AddError("the value must be a valid path.");
        }
      }
    }

    private bool IsValidPath(string path)
    {
      try
      {
        if(Path.GetInvalidPathChars().IsAnyInvalidCharacterInPath(path))
          return false;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
          return Path.IsPathRooted(path) || IsRelativePathWindows(path);
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
          return Path.IsPathRooted(path) || IsRelativePathUnix(path);

      }
      catch (Exception)
      {
        return false;
      }

      return false;
    }

    private bool IsRelativePathUnix(string path)
    {
      return path.StartsWith("..") || path.StartsWith("./") || path.StartsWith("../");
    }

    private bool IsRelativePathWindows(string path)
    {
      return path.StartsWith("..") || path.StartsWith(".\\") || path.StartsWith("..\\");
    }
  }
}