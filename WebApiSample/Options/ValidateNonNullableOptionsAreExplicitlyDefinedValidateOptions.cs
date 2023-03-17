using Microsoft.Extensions.Options;
using Common.Reflection;
using System.Reflection;
using System.Xml.Linq;

namespace WebApiSample.Options
{
    public class ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<TOptions>
        : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly string? _name;
        private readonly IConfiguration _configuration;
        private readonly string _configurationSectionPath;

        public static string UndefinedConfigValueErrorDescription = "was not explicitly defined in loaded configuration file(s).";
        public static string UnexpectedNullConfigValueErrorDescription = "was configured with a value of null in loaded configuration file(s).";

        // TODO: Name is "" for some reason...
        public ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions(string? name, IConfiguration configuration, string configurationSectionPath)
        {
            _name = name; // TODO: Re-investigate cause for this name parameter thingy (something about "named configuration" or some such)
            _configuration = configuration;
            _configurationSectionPath = configurationSectionPath;
        }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (_name != null && _name != name)
            {
                // Ignored if not validating this instance.
                return ValidateOptionsResult.Skip;
            }

            var mainSection = _configuration.GetSection(_configurationSectionPath);
            var errors = HandleOptionsPath(options.GetType(), mainSection);

            if (errors.Any())
                return ValidateOptionsResult.Fail(errors);

            return ValidateOptionsResult.Success;
        }

        private IEnumerable<string> HandleOptionsPath(Type optionType, IConfigurationSection configSection)
        {
            return optionType switch
            {
                object when !optionType.IsNullable() && !configSection.Exists() =>
                    new List<string> { $"Config value '{configSection.Path}' is non-nullable, and {UndefinedConfigValueErrorDescription}" },

                object when optionType.IsNonNullableEnumerable() =>
                    configSection.GetChildren().SelectMany(element => HandleOptionsPath(optionType.GetEnumerableElementType(), element)),

                object when optionType.IsClass && optionType != typeof(string) =>
                    optionType.GetProperties().SelectMany(nestedProperty =>
                    {
                        var subSection = configSection.GetSection(nestedProperty.Name);
                        return HandleOptionsPath(nestedProperty.PropertyType, subSection);
                    }),

                // Placed after IsClass check because IConfigurationSections representing a class structure have Value == null
                object when !optionType.IsNullable() && configSection.Value is null =>
                    new List<string> { $"Config value '{configSection.Path}' is non-nullable, but {UnexpectedNullConfigValueErrorDescription}" },

                _ => new List<string>()
            };
        }
    }
}
