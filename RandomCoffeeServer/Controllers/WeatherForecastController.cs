using Microsoft.AspNetCore.Mvc;

namespace RandomCoffee.Controllers;

[ApiController]
[Route("[controller]")]
public class CoffeeController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<CoffeeController> _logger;

    public CoffeeController(ILogger<CoffeeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public /*IEnumerable<WeatherForecast>*/ string GetCoffee()
    {
        return "I'm a hot coffee!";
    
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            // {
                // Date = DateTime.Now.AddDays(index),
                // TemperatureC = Random.Shared.Next(-20, 55),
                // Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            // })
            // .ToArray();
    }
}