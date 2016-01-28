using System.Web;
using System.Web.Optimization;

namespace BlogDS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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
                      "~/Scripts/respond.js",
                      "~/Scripts/js/bootstrap.min.js",
                      "~/Scripts/jquery.backstretch.min.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/jquery.magnific-popup.min.js",
                      "~/Scripts/jquery.simple-text-rotator.min.js",
                      "~/Scripts/jquery.waypoints.js",
                      "~/Scripts/jquery.countTo.js",
                      "~/Scripts/wow.min.js",
                      "~/Scripts/smoothscroll.js",
                      "~/Scripts/jquery.fitvids.js",
                      "~/Scripts/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/owl.theme.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/magnific-popup.css",
                      "~/Content/simpletextrotator.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css"));
                                  
        }
    }
}
