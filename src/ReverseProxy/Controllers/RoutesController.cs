using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReverseProxy.Models;

namespace ReverseProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutesController : Controller
    {
        private readonly ILogger<RoutesController> _logger;
        private readonly IOptions<RoutingRules> _rulesOptions;

        public RoutesController(ILogger<RoutesController> logger, IOptions<RoutingRules> rulesOptions)
        {
            _logger = logger;
            _rulesOptions = rulesOptions;
        }

        // GET: RoutesController
        [HttpGet]
        public ActionResult Get(string sourcePath)
        {
            if (_rulesOptions.Value.Paths.TryGetValue(sourcePath, out var target))
            {
                return Ok(target);
            }
            
            return NotFound();
        }

        // GET: RoutesController
        [HttpGet("all")]
        public ActionResult List()
        {
            return Ok(_rulesOptions.Value.Paths);
        }

        // POST: RoutesController/
        [HttpPost]
        public ActionResult Create(string sourcePath, string targetPath)
        {
            if (_rulesOptions.Value.Paths.TryGetValue(sourcePath, out _))
            {
                _rulesOptions.Value.Paths[sourcePath] = targetPath;
            }
            else
            {
                _rulesOptions.Value.Paths.Add(sourcePath, targetPath);
            }

            return NoContent();
        }

        // POST: RoutesController/
        [HttpPut]
        public ActionResult Edit(string sourcePath, string targetPath)
        {
            if (_rulesOptions.Value.Paths.TryGetValue(sourcePath, out _))
            {
                _rulesOptions.Value.Paths[sourcePath] = targetPath;
                return NoContent();
            }

            return NotFound();
        }

        // GE: RoutesController/
        [HttpDelete]
        public ActionResult Delete(string sourcePath)
        {
            if (_rulesOptions.Value.Paths.TryGetValue(sourcePath, out _))
            {
                _rulesOptions.Value.Paths.Remove(sourcePath);
                return NoContent();
            }

            return NotFound();
        }
    }
}
