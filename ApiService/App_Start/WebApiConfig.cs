using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApiService.Controllers;
using ApiService.Models;
using Unity;
using Unity.AspNet.WebApi;
using Unity.Injection;
using Unity.Lifetime;

namespace ApiService
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			/*var container = new UnityContainer();
			container.RegisterType<MainController>();
			container.RegisterType<IDal, Dal>(new InjectionConstructor(Properties.Settings.Default.connString));
			config.DependencyResolver = new UnityDependencyResolver(container);*/

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
			
		}
	}
}
