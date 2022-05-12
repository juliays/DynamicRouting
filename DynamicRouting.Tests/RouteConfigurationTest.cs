using Newtonsoft.Json;
using Xunit;
using DynamicRouting;

namespace DynamicRoutingTests
{
    public class RouteConfigTest
    {
        RoutingConfiguration routeConfig;

        public RouteConfigTest()
        {
            routeConfig = JsonConvert.DeserializeObject<RoutingConfiguration>(TestConst.routeConfig);
            routeConfig.Initialize();
        }

        [Fact]
        public void Resolve_Linedata_ShouldFindRouteSpecificHost()
        {
            Route route = getRoute("linedata");

            Assert.NotNull(route);
            Assert.Equal("localhost", route.AllowedHosts[0]);
            Assert.Equal("123", route.XApiKey[0]);
            Assert.Equal("Ingestor.Parser.NewParser", route.Parser);
        }


        [Fact]
        public void Resolve_Magazinedata_ShouldFindRouteAllHosts()
        {
            Route route = getRoute("magazinedata");

            Assert.NotNull(route);
            Assert.Equal("*", route.AllowedHosts[0]);
            Assert.Equal("1234", route.XApiKey[0]);
            Assert.Equal("post", route.Action);

            Assert.Equal("Json", route.Parser);
        }

        [Fact]
        public void Resolve_Linedata2_ShouldNotFindRoute()
        {
            Route route = getRoute("linedata2");

            Assert.Null(route);
        }

        [Fact]
        public void Resolve_Visiondata_ShouldFindRouteCustomizeApiKeyAndController()
        {
            Route route = getRoute("visiondata");

            Assert.NotNull(route);
            Assert.Equal("*", route.AllowedHosts[0]);
            Assert.Equal("123", route.XApiKey[0]);
            Assert.Equal("CSV", route.Parser);
            Assert.Equal("newController", route.Controller);
            Assert.Equal("newMethod", route.Action);
        }

        private Route getRoute(string path)
        {
            foreach (Route route in routeConfig.Routes)
            {
                if (route.Path.Equals(path))
                {
                    return route;
                }
            }
            return null;
        }
    }
}
