using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputValidationLibrary.Validation.Validators;
using InputValidationLibrary.Validation;
using System.Collections.Generic;

namespace InputValidationLibrary.Tests.Validation.Validators
{
  [TestClass]
  public class NotNullValidationRuleTests
  {
    private NotNullValidationRule<TestObject, string> _rule;
    private Dictionary<int, string> _errorMessages;

    [TestInitialize]
    public void Setup()
    {
      _rule = new NotNullValidationRule<TestObject, string>(x => x.Value);
      _errorMessages = new Dictionary<int, string> {{1001, "Value cannot be null."}};
      _rule.ErrorCode = 1001;
    }

    [TestMethod]
    public void Validate_WhenValueIsNotNull_ShouldPass()
    {
      // Arrange
      var testObject = new TestObject {Value = "test"};
      var result = new ValidationResult();

      // Act
      _rule.Validate(testObject, _errorMessages);

      // Assert
      Assert.IsTrue(result.IsValid);
    }

    private class TestObject
    {
      public string Value { get; set; }
    }
  } 
}