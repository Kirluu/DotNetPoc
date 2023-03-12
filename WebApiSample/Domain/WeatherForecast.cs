namespace WebApiSample.Domain
{
    public class WeatherForecast
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required DateTime Date { get; set; }

        public required int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public required string Weekday { get; set; }
    }
}
