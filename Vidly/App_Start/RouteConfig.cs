using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            //Below is the custom route added. This is an old approach and one should use attribute routing
            //Please note routes order matters and should be defined from Specific to generic order 
            //otherwise generic route will be picked up every time and get executed. 
            routes.MapRoute(
                name: "MoviesByReleaseDate",
                url: "movies/released/{year}/{month}",
                defaults: new { Controller = "Movies", action = "ByReleaseDate" }
                //new { year = "\\d{4}", month = "\\d{2}" } regular expression which will ensure year 4 digits and month in 2 digits
                );
            */
            //Below line is the new and preferred way of routing 
            //Using below line you will able to define URL directly over the mvc action 
            //using route attribute example [Route (movies/release/{year}/{month})]
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
