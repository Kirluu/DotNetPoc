using Microsoft.AspNetCore.Mvc;
using WebApiSample.Domain;
using WebApiSample.Dtos;

namespace WebApiSample.CustomMediator
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomMediatorController
    {
        private readonly CreateWeatherForecastCommandHandler _createWeatherForecastCommandHandler;

        public CustomMediatorController(CreateWeatherForecastCommandHandler createWeatherForecastCommandHandler)
        {
            this._createWeatherForecastCommandHandler = createWeatherForecastCommandHandler;
        }

        [HttpPost(Name = "PostWeatherForecastAsRecord")]
        [Consumes("application/json")]
        public async Task<WeatherForecast> PostRecord(CreateWeatherForecastAsRecordCommand request)
        {
            return await _createWeatherForecastCommandHandler.HandleAsync(request);
        }
    }

    public interface IEventBroker
    {
        void Send<T>(T message);
    }

    public class MyLoggingMiddleware
    {
        private readonly ILogger<MyLoggingMiddleware> _logger;

        public MyLoggingMiddleware(ILogger<MyLoggingMiddleware> logger) {
            this._logger = logger;
        }

        public T Run<T>(Func<T> next)
        {
            _logger.LogInformation("Before!");
            next();
            _logger.LogInformation("After!");
        }
    }

    public abstract class MyHandlerServiceBase<TRequest, TResponse>
    {
        public Task<TResponse> HandleAsync(TRequest request)
        {
            // TODO: Instead of newing up middlewares manually, we would inject IServiceProvider and find all implementations of some IMiddleware interface.
            var action = () => Handle(request);
            var middleware1 = new MyLoggingMiddleware(null);
            return middleware1.Run(action);
        }

        protected abstract Task<TResponse> Handle(TRequest request);
    }

    public abstract class MyHandlerServiceBase<TRequest>
    {
        protected IEventBroker EventBroker { get; }

        public MyHandlerServiceBase(IEventBroker eventBroker)
        {
            EventBroker = eventBroker;
        }

        protected abstract Task Handle(TRequest request);
    }

    public class CreateWeatherForecastCommandHandler : MyHandlerServiceBase<CreateWeatherForecastAsRecordCommand, WeatherForecast>
    {
        protected override Task<WeatherForecast> Handle(CreateWeatherForecastAsRecordCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
