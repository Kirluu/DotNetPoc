using FluentValidation;
using Microsoft.Extensions.Options;

namespace WebApiSample.Options
{
    public static class OptionsServiceCollectionExtensions
    {
        public static void BindOptionsTypeToSection<TOptions>(this IServiceCollection services, string configurationKey)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(configurationKey)
                .ValidateDataAnnotations()
                .ValidateFluentValidation()
                .ValidateNonNullableValueTypesExplicitlyDefined(configurationKey)
                .ValidateOnStart();

            // Explicitly register the settings object by delegating to the IOptions object
            services.AddSingleton(resolver =>
                    resolver.GetRequiredService<IOptions<TOptions>>().Value);
        }

        public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
            this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                provider => new FluentValidationValidateOptions<TOptions>(
                  optionsBuilder.Name, provider));
            return optionsBuilder;
        }

        public static OptionsBuilder<TOptions> ValidateNonNullableValueTypesExplicitlyDefined<TOptions>(
            this OptionsBuilder<TOptions> optionsBuilder, string configurationSectionPath) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                provider => new ValidateNonNullableOptionsAreExplicitlyDefinedValidateOptions<TOptions>(
                  optionsBuilder.Name, provider.GetRequiredService<IConfiguration>(), configurationSectionPath));
            return optionsBuilder;
        }
    }
}
