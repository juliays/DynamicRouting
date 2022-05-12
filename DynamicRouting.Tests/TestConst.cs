using System;
namespace DynamicRoutingTests
{
	public class TestConst
	{
        public const string routeConfig = @"{
            ""Controller"": ""ingestor"",
            ""Action"": ""post"",
            ""AllowedHosts"": [""*""],
            ""XApiKey"": [""1234""],
            ""Routes"": [
              {
                ""Path"": ""visiondata"",
                ""Parser"": ""CSV"",
                ""Controller"": ""newController"",
                ""Action"": ""newMethod"",
                ""XApiKey"": [""123""],
              },
              {
                ""Path"": ""magazinedata"",
                ""Parser"": ""Json""
              },
              {
                ""AllowedHosts"": [""localhost""],
                ""XApiKey"": [""123""],
                ""Path"": ""linedata"",
                ""Parser"": ""Ingestor.Parser.NewParser""
              }
            ]
        }";
    }
}

