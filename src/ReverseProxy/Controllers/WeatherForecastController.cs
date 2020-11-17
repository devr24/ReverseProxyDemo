using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReverseProxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi;

namespace ReverseProxy.Controllers
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
        private readonly IOptions<RoutingRules> _rulesOptions;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<RoutingRules> rulesOptions)
        {
            _logger = logger;
            _rulesOptions = rulesOptions;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class SwaggerReaderController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get(string uri)
        {
            using var httpClient = new HttpClient();

            var str = await httpClient.GetStringAsync(uri);

            // Read V3 as YAML
            var openApiDocument = new OpenApiStringReader().Read(str, out var diagnostic); // new OpenApiStreamReader().Read(stream, out var diagnostic);

            // Write V2 as JSON
            var outputString = openApiDocument.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            return Ok(openApiDocument);
        }
    }
}
