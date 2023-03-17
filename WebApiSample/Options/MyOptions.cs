using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Options
{
    public class MyOptions
    {
        public required int MyNumber { get; set; } // TODO: How can we - using FluentValidation or DataAnnotations or custom OptionsValidator logic - make sure that non-nullable int means that the field should "be defined" (a la JsonRequired attribute)?
        public required string MyUrl { get; set; }
        public required string? MyNullableString { get; set; }
        public required string MyString { get; set; }
        public required MyInnerObject MyInnerObject { get; set; }
        public required int[] MyArrayOfInts { get; set; }
        public required List<string> MyArrayOfStrings { get; set; }
        public required MyInnerObject[] MyArrayOfObjects { get; set; }
    }

    public class MyInnerObject
    {
        public required int MyInnerNumber { get; set; }
        public required string MyInnerString { get; set; }
    }

    public class MyOptionsValidator : AbstractValidator<MyOptions>
    {
        public MyOptionsValidator()
        {
            RuleFor(x => x.MyUrl)
                .NotEmpty();
            RuleFor(x => x.MyUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.MyUrl))
                .WithMessage((_, v) => $"'{v}' is not a valid Url");
        }
    }

}
