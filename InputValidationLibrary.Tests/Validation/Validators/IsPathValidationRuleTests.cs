using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputValidationLibrary.Validation;
using InputValidationLibrary.Validation.Validators;
using InputValidationLibrary.Validation.ErrorMessages;
using System.Collections.Generic;

namespace InputValidationLibrary.Tests.Validation.Validators
{
    [TestClass]
    public class IsPathValidationRuleTests
    {
        private IsPathValidationRule<TestObject> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new IsPathValidationRule<TestObject>(x => x.PathValue);
            _errorMessages = new Dictionary<int, string> { { 1007, "Value must be a valid path." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1007;
        }

        [TestMethod]
        public void Validate_WhenPathIsValid_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { PathValue = @"C:\Valid\Path\To\File.txt" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenPathIsValidRelativePath_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { PathValue = @"..\Valid\Relative\Path" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenPathIsInvalid_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { PathValue = @"C:\Invalid|Path\To\File.txt" }; // Contains invalid character '|'
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be a valid path.", result.Errors[0]);
        }

        [TestMethod]
        public void Validate_WhenPathIsNull_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { PathValue = null };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid); // Null should pass since it's not invalid unless required by another rule
        }

        [TestMethod]
        public void Validate_WhenPathIsEmpty_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { PathValue = string.Empty };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid); // Empty string should pass as it's a valid edge case
        }

        [TestMethod]
        public void Validate_WhenValueIsNotAPath_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { PathValue = "hello world" }; // Clearly not a valid path
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be a valid path.", result.Errors[0]);
        }

        private class TestObject
        {
            public string PathValue { get; set; }
        }
    }
}