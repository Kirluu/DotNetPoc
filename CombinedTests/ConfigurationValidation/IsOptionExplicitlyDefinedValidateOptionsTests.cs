using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSample.Options;

namespace CombinedTests.ConfigurationValidation
{
    public class IsOptionExplicitlyDefinedValidateOptionsTests
    {
        private TestOptions DummyTestOptions = new TestOptions { Number = 0, String = "abc", Url = "https://www.abc.com", ArrayOfInts = new int[] { 1, 2 }, InnerObject = new InnerTestOptions { InnerNumber = 0, InnerString = "abc" }, ListOfStrings = new List<string> { "a" }, ArrayOfInnerObjects = new InnerTestOptions[] { new InnerTestOptions { InnerNumber = 0, InnerString = "abc" } } };

        [Theory]
        [InlineData("MyOptions:Number")]
        [InlineData("MyOptions:Url")]
        [InlineData("MyOptions:String")]
        [InlineData("MyOptions:InnerObject")]
        [InlineData("MyOptions:ArrayOfInts")]
        [InlineData("MyOptions:ListOfStrings")]
        [InlineData("MyOptions:ArrayOfInnerObjects")]
        public void TestIsOptionExplicitlyDefinedValidateOptions_WhenNothingIsDefined_ReportsErrorFor(string propertyNameWithError)
        {
            // Arrange
            var settings = EmptyObject;
            var sut = GetIsOptionExplicitlyDefinedValidateOptions(settings);

            // Act
            var validationResult = sut.Validate("", DummyTestOptions);

            // Assert
            var errorExists = validationResult.Failures?.Any(f => f.Contains(propertyNameWithError) && f.Contains(ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<object>.UndefinedConfigValueErrorDescription)) ?? false;
            Assert.True(errorExists, $"Errors: {string.Join(Environment.NewLine, validationResult.Failures ?? new List<string>())}");
        }

        [Theory]
        [InlineData("MyOptions:InnerObject:InnerNumber")]
        [InlineData("MyOptions:InnerObject:InnerString")]
        public void TestIsOptionExplicitlyDefinedValidateOptions_WhenInnerObjectWithNothingDefined_ReportsError(string propertyNameWithError)
        {
            // Arrange
            var settings = new
            {
                InnerObject = EmptyObject
            };
            var sut = GetIsOptionExplicitlyDefinedValidateOptions(settings);

            // Act
            var validationResult = sut.Validate("", DummyTestOptions);

            // Assert
            var errorExists = validationResult.Failures?.Any(f => f.Contains(propertyNameWithError) && f.Contains(ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<object>.UndefinedConfigValueErrorDescription)) ?? false;
            Assert.True(errorExists, $"Errors: {string.Join(Environment.NewLine, validationResult.Failures ?? new List<string>())}");
        }

        [Theory]
        [InlineData("MyOptions:ArrayOfInnerObjects:0:InnerNumber")]
        [InlineData("MyOptions:ArrayOfInnerObjects:0:InnerString")]
        [InlineData("MyOptions:ArrayOfInnerObjects:2:InnerNumber")]
        [InlineData("MyOptions:ArrayOfInnerObjects:2:InnerString")]
        public void TestIsOptionExplicitlyDefinedValidateOptions_WhenArrayOfTwoInnerObjectsWithNothingDefined_ReportsError(string propertyNameWithError)
        {
            // Arrange
            var settings = new
            {
                ArrayOfInnerObjects = new object[]
                {
                    EmptyObject,
                    new { },
                    EmptyObject
                }
            };
            var sut = GetIsOptionExplicitlyDefinedValidateOptions(settings);

            // Act
            var validationResult = sut.Validate("", DummyTestOptions);

            // Assert
            var errorExists = validationResult.Failures?.Any(f => f.Contains(propertyNameWithError) && f.Contains(ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<object>.UndefinedConfigValueErrorDescription)) ?? false;
            Assert.True(errorExists, $"Errors: {string.Join(Environment.NewLine, validationResult.Failures ?? new List<string>())}");
        }

        [Fact]
        public void TestIsOptionExplicitlyDefinedValidateOptions_WhenAllOptionsHaveExplicitlyDefinedValues_ReportsNoErrors()
        {
            // Arrange
            var sut = GetIsOptionExplicitlyDefinedValidateOptions(DummyTestOptions);

            // Act
            var validationResult = sut.Validate("", DummyTestOptions);

            // Assert
            Assert.Empty(validationResult.Failures ?? new List<string>());
        }

        private object EmptyObject = new
        {
            Dummy = 123 // Empty object is equivalent to null, so must define some dummy property
        };

        private ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<TestOptions> GetIsOptionExplicitlyDefinedValidateOptions(object settings)
        {
            var json = JsonConvert.SerializeObject(new
            {
                MyOptions = settings
            });
            var config = ConfigurationTestHelper.GetConfigurationFromJson(json);
            var sut = new ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<TestOptions>("", config, "MyOptions");
            return sut;
        }
    }
}
