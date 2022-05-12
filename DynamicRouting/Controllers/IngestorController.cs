using System;
using System.Net.Http;
using System.Threading.Tasks;
using DynamicRouting.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicRouting.Controllers
{
	public class IngestorController : Controller
	{

        private readonly ILogger<IngestorController> _logger;
        public IngestorController(ILogger<IngestorController> logger)
		{
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> PostRequest()
        {
            var parserInputTypeString = ControllerContext.HttpContext.Items["Parser"]?.ToString();

            if (string.IsNullOrWhiteSpace(parserInputTypeString))
            {
                _logger.LogWarning("No parser for uri {0} is configured ", ControllerContext.HttpContext.Request.RouteValues["Route"]);
                return StatusCode(StatusCodes.Status404NotFound, "No parser found");
            }

            try
            {
                var body = await ControllerContext.HttpContext.Request.GetRawBodyAsync();

                _logger.LogDebug("received message {0}\n {1}", parserInputTypeString, body);
                return Ok("IngestController::PostRequest - \n "+ parserInputTypeString + "\n" + body);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }

        }
    }

}

