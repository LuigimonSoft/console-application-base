using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.Validators;
using InputValidationLibrary.Validation.ErrorMessages;
using System.Collections.Generic;

namespace InputValidationLibrary.Tests.Validation.Validators
{
    [TestClass]
    public class IsNumericValidationRuleTests
    {
        private IsNumericValidationRule<TestObject> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new IsNumericValidationRule<TestObject>(x => x.Value);
            _errorMessages = new Dictionary<int, string> { { 1003, "Value must be numeric." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1003;
        }

        [TestMethod]
        public void Validate_WhenValueIsNumeric_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { Value = "123" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result, _errorMessages);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsNotNumeric_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { Value = "abc" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result, _errorMessages);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be numeric.", result.Errors[0]);
        }

        [TestMethod]
        public void Validate_WhenValueIsNull_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { Value = null };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result, _errorMessages);

            // Assert
            Assert.IsTrue(result.IsValid); // Null value should pass for IsNumeric rule
        }

        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}
