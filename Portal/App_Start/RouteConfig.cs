using Portal.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*****************************************************/
            /***  Routing setup for Angular without #          ***/
            /***  https://www.youtube.com/watch?v=4keNnVxOh_M  ***/
            /*****************************************************/

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
				{
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				},
				constraints: new 
				{
				  serverRoute = new ServerRouteConstraint(url =>
					{
						return url.PathAndQuery.StartsWith("/Api/", StringComparison.CurrentCultureIgnoreCase);
					})
				});

            routes.MapRoute(
                name: "pdf",
                url: "{controller}/{action}/{id}",
                defaults: new 
				{ 
					controller = "Home", 
					action = "Index", 
					id = UrlParameter.Optional 
				},
				constraints: new
				{
					serverRoute = new ServerRouteConstraint(url =>
					{
						 return url.PathAndQuery.StartsWith("/pdf/", StringComparison.CurrentCultureIgnoreCase);
					})
				});
           
			routes.MapRoute(
				name: "angular",
				url: "{*url}",
				defaults: new 
				{ 
					controller = "home", 
					action = "index" 
				});
        }
    }
}
