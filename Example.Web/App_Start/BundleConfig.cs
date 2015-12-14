using System.Web;
using System.Web.Optimization;

namespace Example.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Example/styles").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/animate.css",
                      "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Example/anonymous/styles").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/Content/animate.css",
                "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/Example/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Scripts/inspinia.js",
                "~/Scripts/plugins/pace/pace.min.js",
                "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/Example/anonymous/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Scripts/inspinia.js"));

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

        }
    }
}
