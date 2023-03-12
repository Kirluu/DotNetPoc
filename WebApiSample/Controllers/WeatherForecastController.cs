using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Domain;
using WebApiSample.Dtos;

namespace WebApiSample.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("WebApiSample checking in!");
            await Task.Delay(1000);

            var middleServiceHttpClient = new HttpClient();
            middleServiceHttpClient.BaseAddress = new Uri("http://localhost:1234");

            return await middleServiceHttpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast") ?? new List<WeatherForecast>();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        [Consumes("application/json")]
        public async Task<WeatherForecast> Post(CreateWeatherForecastWithRequiredPropertiesCommand request)
        {
            return await _mediator.Send(request);
        }


        [HttpPost(Name = "PostWeatherForecastAsRecord")]
        [Consumes("application/json")]
        public async Task<WeatherForecast> PostRecord(CreateWeatherForecastAsRecordCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}