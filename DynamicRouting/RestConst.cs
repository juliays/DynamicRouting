using System;
using System.Diagnostics.CodeAnalysis;

namespace DynamicRouting
{
    [ExcludeFromCodeCoverage]
    public class RestConst
    {
        public const string ALLOWED_HOSTS = "AllowedHosts";
        public const string PARSER = "Parser";
        public const string ROUTING_PATH = "path";
        public const string ROUTING_CONTROLLER = "controller";
        public const string ROUTING_ACTION = "action";


        public const string CONFIG_ROUTING = "Routing";

        public const string HEADER_HOST = "Host";
        public const string HEADER_API_KEY = "XApiKey";

        public const string ERROR_NO_APIKEY_PRESENT_IN_HEADER = "Api Key was not provided in Header";
        public const string ERROR_NO_HOST_PRESENT_IN_HEADER = "No Host variable being passed in Header";

        public const string ERROR_APIKEY_NOT_MATCHING = "Unauthorized client";
        public const string ERROR_HOST_NOT_ALLOWED = "Host not on AllowedHosts list";
        public const string ERROR_NO_ROUTE_CONFIGURED = "No route is configured for current path";
        public const string ERROR_UKNOWN_PARSER_TYPE = "Parser input type not found";
        public const string ERROR_NO_PARSER_CONFIGURED = "Parser not configured.";
        public const string ERROR_GRPC_CALL_FAILURE = "Grpc call failed.";

        public const string SCHEMA_API_KEY_VALIDATION = "APIKeyValidation";

        public const string REST_REQUEST_RECEIVED_COUNTER = "RestRequestReceived";
        public const string REST_REQUEST_PROCESSED_OK_COUNTER = "RestRequestProcessedOK";
        public const string REST_REQUEST_PROCESSED_ERROR_COUNTER = "RestRequestProcessedError";
        public const string REST_REQUEST_FAILED_AUTH_COUNTER = "RestRequestFailedAuth";
        public const string REST_REQUEST_FAILED_MISC_COUNTER = "RestRequestFailedMisc";
        public const string REST_200_RESPONSE_TIME_GAUGE = "Rest200ResponseTime";

        public const string METRICS_LABEL = "endpoint";
    }
}

