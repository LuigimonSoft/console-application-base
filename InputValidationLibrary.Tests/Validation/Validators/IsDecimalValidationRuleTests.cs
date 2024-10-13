using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.Validators;
using InputValidationLibrary.Validation.ErrorMessages;
using System.Collections.Generic;

namespace InputValidationLibrary.Tests.Validation.Validators
{
    [TestClass]
    public class IsDecimalValidationRuleTests
    {
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _errorMessages = new Dictionary<int, string>
            {
                { 4001, "The value must be a valid decimal." }
            };
            ErrorMessageStore.Messages = _errorMessages;
        }

        [TestMethod]
        public void Validate_WhenValueIsValidDecimal_ShouldPass()
        {
            // Arrange
            var rule = new IsDecimalValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 4001
            };
            var testObject = new TestObject { Value = "123.45" };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsInvalidDecimal_ShouldFail()
        {
            // Arrange
            var rule = new IsDecimalValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 4001
            };
            var testObject = new TestObject { Value = "not_a_decimal" };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The value must be a valid decimal.", result.Errors[0]);
        }

        [TestMethod]
        public void Validate_WhenValueIsInteger_ShouldPass()
        {
            // Arrange
            var rule = new IsDecimalValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 4001
            };
            var testObject = new TestObject { Value = "123" };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid); // Should pass as integers are valid decimals
        }

        // Helper class to use in tests
        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}