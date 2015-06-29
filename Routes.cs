using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace MainBit.Captcha
{
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "mainbit/captcha",
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Captcha"},
                                                                                      { "controller", "CaptchaImage" },
                                                                                      { "action", "GetCaptchaImage" }
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Captcha"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}