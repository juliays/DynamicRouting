using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace DynamicRouting.security
{
    public class APIKeyValidator
    {
        private readonly RequestDelegate _next;
        private const string AuthorizationHeaderName = "XApiKey";
        private const string ANY_HOST = "*";

        private readonly ILogger<APIKeyValidator> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public APIKeyValidator(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<APIKeyValidator>();
        }

        public async Task InvokeAsync(HttpContext Context)
        {
            if (!Context.Request.Headers.TryGetValue(AuthorizationHeaderName, out var extractedApiKey))
            {
                await ErrorReturn(Context, StatusCodes.Status401Unauthorized, RestConst.ERROR_NO_APIKEY_PRESENT_IN_HEADER);
                return;
            }

            var allowedHost = Context.Items[RestConst.ALLOWED_HOSTS];

            if (allowedHost is not string[] allowHostArray)
            {
                await ErrorReturn(Context, StatusCodes.Status404NotFound, RestConst.ERROR_NO_ROUTE_CONFIGURED);
                return;
            }

            var apiKey = Context.Items[RestConst.HEADER_API_KEY];

            if (!allowHostArray.Any(s => s.Equals(ANY_HOST, StringComparison.InvariantCultureIgnoreCase)))
            {
                string requestedDomain = Context.Request.Headers[RestConst.HEADER_HOST];
                if (string.IsNullOrWhiteSpace(requestedDomain))
                {
                    await ErrorReturn(Context, StatusCodes.Status401Unauthorized, RestConst.ERROR_NO_HOST_PRESENT_IN_HEADER);
                    return;
                }
                string host = requestedDomain.Split(":")[0];

                if (!Validation(((string[])allowedHost), host))
                {
                    await ErrorReturn(Context, StatusCodes.Status401Unauthorized, RestConst.ERROR_HOST_NOT_ALLOWED);
                    return;
                }
            }
            if (!Validation(((string[])apiKey), extractedApiKey[0]))
            {
                await ErrorReturn(Context, StatusCodes.Status401Unauthorized, RestConst.ERROR_APIKEY_NOT_MATCHING);
                return;
            }

            await _next(Context);

        }

        private static bool Validation(string[] acceptableValues, string input)
        {
            return acceptableValues.Any(s => !string.IsNullOrWhiteSpace(s) && s.Trim().Equals(input, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task ErrorReturn(HttpContext context, int status, string errorMessage)
        {
            _logger.LogError("{Path} - {ErrorMessage}", context.Request.Path, errorMessage);

            string[] values = { context.Request.Path, errorMessage };
            _logger.LogError("{0} - {1}", values);
            context.Response.StatusCode = status;
            await context.Response.WriteAsync(errorMessage);
        }

    }


}

