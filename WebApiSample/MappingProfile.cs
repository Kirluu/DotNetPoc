using AutoMapper;
using WebApiSample.Domain;
using WebApiSample.Dtos;

namespace WebApiSample
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<CreateWeatherForecastWithRequiredPropertiesCommand, WeatherForecast>()
                .ForMember(x => x.Id, o => o.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.Weekday, o => o.Ignore());
            CreateMap<WeatherForecast, WeatherForecastDto>();
        }
    }
}
