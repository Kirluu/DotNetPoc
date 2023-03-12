using FluentValidation;
using MediatR;

namespace WebApiSample.Dtos
{
    public record CreateWeatherForecastAsRecordCommand(DateTime Date, int TemperatureC, string? Summary) : IRequest<WeatherForecastDto>
    {
        // NOTE: We can't use "required" keyword in the constructor parameter syntax, so we can't make use of the nice built-in validation of the presence of "required" properties being part of a posted JSON body.
    }

    public class CreateWeatherForecastAsRecordCommandHandler : AbstractValidator<CreateWeatherForecastAsRecordCommand>, IRequestHandler<CreateWeatherForecastAsRecordCommand, WeatherForecastDto>
    {
        public CreateWeatherForecastAsRecordCommandHandler()
        {
            RuleFor(x => x.TemperatureC).NotEmpty();
        }

        public Task<WeatherForecastDto> Handle(CreateWeatherForecastAsRecordCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new WeatherForecastDto { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 22, Weekday = "Monday" });
        }
    }

    //public class CreateWeatherForecastAsRecordCommandValidator : AbstractValidator<CreateWeatherForecastAsRecordCommand>
    //{
    //    public CreateWeatherForecastAsRecordCommandValidator()
    //    {
    //        RuleFor(x => x.TemperatureC).NotEmpty();
    //    }
    //}
}
