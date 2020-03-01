using System.Web;
using System.Web.Mvc;

namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //Below line apply authorize filter globally 
            filters.Add(new AuthorizeAttribute());
            //With below line my application end points will no longer  
            //be available on http channel 
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
