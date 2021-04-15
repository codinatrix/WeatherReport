using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using Unity;
using WeatherReport.Web.Integration.SMHI;

namespace WeatherReport.Web
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
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Return Json as default instead of xml
            // (Since we are using pre .Net Core)
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            var container = new UnityContainer();
            config.DependencyResolver = new UnityResolver(container);

            container.RegisterType<ISMHIGateway, SMHIGateway>();
            container.RegisterType<ITemperatureService, TemperatureService>();
        }
    }
}
