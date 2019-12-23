using System.Web.Http;

namespace RestMediaServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{type}",
                defaults: new { id = RouteParameter.Optional, type = RouteParameter.Optional }
            );
        }
    }
}
