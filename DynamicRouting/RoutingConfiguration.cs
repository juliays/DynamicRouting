using System;
using System.Collections.Generic;

namespace DynamicRouting
{
    public class RoutingConfiguration
    {
        // dynamic routing config
        public string Controller { get; set; }
        public string Action { get; set; }
        public string[] AllowedHosts { get; set; }
        public string[] XApiKey { get; set; }
        public List<Route> Routes { get; set; }

        public void Initialize()
        {
            foreach (Route route in Routes)
            {
                route.Controller = string.IsNullOrWhiteSpace(route.Controller) ? Controller : route.Controller;
                route.Action = string.IsNullOrWhiteSpace(route.Action) ? Action : route.Action;
                route.AllowedHosts ??= AllowedHosts;
                route.XApiKey ??= XApiKey;
            }
        }

    }

    public class Route
    {
        public string Path { get; set; }
        public string Parser { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string[] AllowedHosts { get; set; }
        public string[] XApiKey { get; set; }
    }
}

