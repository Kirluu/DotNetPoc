using WebApiSample.Domain;

namespace WebApiSample.Dtos
{
    public class WeatherForecastDto : WeatherForecast
    {

        public WeatherForecastDto()
        {
        }

        public WeatherForecastDto(WeatherForecast entity) {
            Id = entity.Id;
            this.Date = entity.Date;
            this.TemperatureC = entity.TemperatureC;
            this.Weekday = entity.Weekday;
            this.Summary = entity.Summary;
        }
    }
}
