using System.Web;
using System.Web.Optimization;

namespace Vidly
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*let us consolidate below bundle to include Jquery, bootstrap and third pary libs 
            like data tables jquery plugin . 
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            */
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                    "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/datatables/jquery.datatables.js",
                       "~/Scripts/datatables/datatables.bootstrap.js",
                      "~/Scripts/typeahead.bundle.js",
                       "~/Scripts/toastr.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /* since consolidate above so no longer required - hence commented 
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootbox.js",
                      "~/Scripts/bootstrap.js"));
             */
            //To use a different boot strap theme go to bootswatch.com and choose theme
            //save it under contents folder with theme name suffix to distinguish from original css 
            //file and replace bootstrap.css with its name in below code
            //I have used bootstrap-lumen.css in place of  "~/Content/bootstrap.css",
            //We have added datatable bootstrap to apply bootstrap table styles 
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/datatables/css/datables.bootstrap.css",
                      "~/Content/typeahead.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css"));

        }
    }
}
