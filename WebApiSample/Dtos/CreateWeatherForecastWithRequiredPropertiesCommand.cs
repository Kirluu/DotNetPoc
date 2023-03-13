using AutoMapper;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using WebApiSample.Domain;

namespace WebApiSample.Dtos
{
    public class CreateWeatherForecastWithRequiredPropertiesCommand : IRequest<WeatherForecastDto>
    {
        public required DateTime Date { get; set; }

        // NOTE: Can I have a DTO with no required and use AutoMapper to create a Domain entity which has required on a property that is auto-mapped? (In a way, the property is assigned by AutoMapper, but in this case, the required-ness doesn't "correctly" / automagically transfer to the DTO which may have not had its created value created in a "required" way)
        [JsonRequired]
        public int TemperatureC { get; set; }

        [JsonRequired]
        public string? Summary { get; set; }

        public WeatherForecast ToDomain()
        {
            return new WeatherForecast { Date = Date, TemperatureC = TemperatureC, Weekday = "Monday", Summary = Summary };
        }
    }

    public class CreateWeatherForecastCommandHandler : IRequestHandler<CreateWeatherForecastWithRequiredPropertiesCommand, WeatherForecastDto>
    {
        private readonly IMapper _mapper;

        public CreateWeatherForecastCommandHandler(IMapper mapper) {
            this._mapper = mapper;
        }

        public Task<WeatherForecastDto> Handle(CreateWeatherForecastWithRequiredPropertiesCommand request, CancellationToken cancellationToken)
        {
            var wf = _mapper.Map<WeatherForecast>(request);

            return Task.FromResult(new WeatherForecastDto { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 22, Weekday = "Monday" });
        }
    }

    public class CreateWeatherForecastCommandValidator : AbstractValidator<CreateWeatherForecastWithRequiredPropertiesCommand> { 
        public CreateWeatherForecastCommandValidator()
        {
            //RuleFor(x => x.TemperatureC).NotEmpty();
        }
    }
}
