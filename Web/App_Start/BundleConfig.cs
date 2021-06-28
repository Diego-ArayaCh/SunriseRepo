using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        //Se definen complementos a usar en scripts y content


        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                   "~/Scripts/jquery-{version}.js"
                   ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // SweetAlert
            bundles.Add(new ScriptBundle("~/bundles/sweetalert").Include("~/Scripts/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include("~/Scripts/jquery.unobtrusive*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
             "~/Scripts/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      // "~/Content/bootstrap.css", //Bootstrap original
                      //"~/Content/site.css", //Hacer uso de este CSS
                      "~/Content/bootstrap-Lux.min.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/sweetalert.css"
                      )); //Tema de bootstrap

        }
    }
}
