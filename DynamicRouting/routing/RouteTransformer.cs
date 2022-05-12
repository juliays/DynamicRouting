using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace DynamicRouting.routing
{
    public class RouteTransformer : DynamicRouteValueTransformer
    {
        private readonly IOptions<RoutingConfiguration> _routeConfigs;

        public RouteTransformer(IOptions<RoutingConfiguration> routeConfigs)
        {
            _routeConfigs = routeConfigs;
            _routeConfigs.Value.Initialize();
        }
        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {

            if (!values.ContainsKey(RestConst.ROUTING_PATH))
            {
                return ValueTask.FromResult(values);
            }

            var path = (string)values[RestConst.ROUTING_PATH];

            var route = _routeConfigs.Value.Routes.FirstOrDefault(c => c.Path.Equals(path, System.StringComparison.InvariantCultureIgnoreCase));

            if (route != null)
            {
                values[RestConst.ROUTING_CONTROLLER] = route.Controller;
                values[RestConst.ROUTING_ACTION] = route.Action;

                httpContext.Items.Add(RestConst.PARSER, route.Parser);
                httpContext.Items.Add(RestConst.ALLOWED_HOSTS, route.AllowedHosts);
                httpContext.Items.Add(RestConst.HEADER_API_KEY, route.XApiKey);
            }

            return ValueTask.FromResult(values);

        }
    }
}

