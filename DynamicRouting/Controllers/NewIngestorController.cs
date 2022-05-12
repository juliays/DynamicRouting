using System;
using System.Net.Http;
using System.Threading.Tasks;
using DynamicRouting.Extension;
using DynamicRouting.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicRouting.Controllers
{
	public class NewIngestorController : Controller
	{
		ILogger<NewIngestorController> _logger;

		public NewIngestorController(ILogger<NewIngestorController> logger)
		{
			_logger = logger;
		}

        [HttpPost]
        public IActionResult Calculate([FromBody] TestData input)
        {
            var parserInputTypeString = ControllerContext.HttpContext.Items["Parser"]?.ToString();

            if (string.IsNullOrWhiteSpace(parserInputTypeString))
            {
                _logger.LogWarning("No parser for uri {0} is configured ", ControllerContext.HttpContext.Request.RouteValues["Route"]);
                return StatusCode(StatusCodes.Status404NotFound, "No parser found");
            }

            try
            {
                _logger.LogInformation("==========input is {0}", input.textField);

                return Ok("NewIngestorController::Calculate " + input.textField);

            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }

        }
    }
}

