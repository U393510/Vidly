using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Vidly
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*
             * In Api result you can see properties are shown in Pascal Notation 
             * i.e. first loader of every word is upper case but in Javascript 
             * we use Camel notation so first loader of every word should be 
             * lowercase and first letter afterwards should be upper case 
             * so in current case we try to consume this output in javascript 
             * our code become little bit ugly so let us see how to configure 
             * webapi to return Json object using Camel Notation 
             */
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            //to enable camel casing we need to set the couple of properties 
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
