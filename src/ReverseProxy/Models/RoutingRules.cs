using System.Collections.Generic;

namespace ReverseProxy.Models
{
    public class RoutingRules
    {
        public bool BlockUnknownPaths { get; set; }
        public Dictionary<string, string> Paths { get; set; }
    }
}
