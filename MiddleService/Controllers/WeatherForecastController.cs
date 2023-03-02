using Microsoft.AspNetCore.Mvc;

namespace MiddleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("MiddleService checking in!");
            await Task.Delay(1000);

            var middleServiceHttpClient = new HttpClient();
            middleServiceHttpClient.BaseAddress = new Uri("http://localhost:9999");

            return await middleServiceHttpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast") ?? new List<WeatherForecast>();
        }
    }
}