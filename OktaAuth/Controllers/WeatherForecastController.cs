using Microsoft.AspNetCore.Mvc;

namespace OktaAuth.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Okta.IJwtValidator _validationService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public WeatherForecastController(ILogger<WeatherForecastController> logger, Okta.IJwtValidator validationService)
        {
            _logger = logger;
            _validationService = validationService;
        }

    
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            //get token from header
            var authToken = this.HttpContext.Request.Headers["Authorization"].ToString();

            if (String.IsNullOrEmpty(authToken))
            {
                return Unauthorized();
            }

            //validate the token
            var validatedToken = await _validationService.ValidateToken(authToken.Split(" ")[1]);

            if (validatedToken == null)
            {
                return Unauthorized();
            }

            return new JsonResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
