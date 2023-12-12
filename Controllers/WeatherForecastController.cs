using Azure_Cache_For_Redis_WebAPI_Example.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Cache_For_Redis_WebAPI_Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IRedisCache _redisCache;


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRedisCache redisCache)
        {
            _logger = logger;
            _redisCache = redisCache;

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var cacheData = await _redisCache.GetCacheData<List<WeatherForecast>>("weather");
            
            if(cacheData != null) {
              return Ok(cacheData);
            }
           
            var dataFromDb =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            await _redisCache.SetCacheData("weather", dataFromDb,  DateTimeOffset.Now.AddMinutes(5));

            return Ok(dataFromDb);

        }
    }
}