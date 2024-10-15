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
            var testObject = new TestObject { Value = new decimal(123.45) };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        private class TestObject
        {
            public decimal Value { get; set; }
        }
    }
}