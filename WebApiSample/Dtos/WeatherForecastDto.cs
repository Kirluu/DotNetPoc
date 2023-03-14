using WebApiSample.Domain;

namespace WebApiSample.Dtos
{
    public class WeatherForecastDto // : WeatherForecast // TODO: It is NOT allowed to inherit from the Domain layer, when defining DTOs. At least with Swashbuckle, it resulted in OpenApi doc of both the DTO as well as the inherited Domain entity (the DTO just had a ref to the Domain entity (inheritance support I guess?)).
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required DateTime Date { get; set; }

        public required int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // TODO: This field gets described as "required" in OpenApi doc by "RequireNonNullablePropertiesSchemaFilter", even though it's an OUTPUT dto, which we probably don't want...

        public string? Summary { get; set; }

        public required string Weekday { get; set; }

        public WeatherForecastDto() { }

        public WeatherForecastDto(WeatherForecast entity) {
            Id = entity.Id;
            this.Date = entity.Date;
            this.TemperatureC = entity.TemperatureC;
            this.Weekday = entity.Weekday;
            this.Summary = entity.Summary;
        }
    }
}
