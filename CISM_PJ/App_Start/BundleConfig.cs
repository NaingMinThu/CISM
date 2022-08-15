using System.Web;
using System.Web.Optimization;

namespace CISM_PJ
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryValidate").Include("~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.validate-additional-methods.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/popper").Include("~/Scripts/popper.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/toastr.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/app").Include("~/Scripts/app.js"));
            bundles.Add(new ScriptBundle("~/bundles/pace").Include("~/Scripts/pace.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/holder").Include("~/Scripts/holder.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/perfect-scrollbar").Include("~/Scripts/perfect-scrollbar.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/coreui-js").Include("~/Scripts/coreui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryDataTable").Include("~/Scripts/jquery.dataTables.min.js",
                "~/Scripts/dataTables.bootstrap4.min.js",
                "~/Scripts/jquery.dataTable-date-eu.js",
                  "~/Scripts/jquery.dataTable-formatted-numbers.js"));
            //  bundles.Add(new ScriptBundle("~/bundles/gijgo").Include("~/Scripts/gijgo/combined/gijgo.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            //   CoreUI Theme
            bundles.Add(new StyleBundle("~/Content/coreui-all-icons").Include(
                "~/Content/coreui-icons.css",
                "~/Content/font-awesome.min.css",
                "~/Content/open-iconic-bootstrap.css",
                "~/Content/simple-line-icons.css",
                "~/Content/flag-icon.min.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/style.css",
                "~/Content/PagedList.css",
                "~/Content/toastr.min.css",
                "~/Content/dataTables.bootstrap4.min.css",
                 "~/Content/pace-theme-minimal.css"
                ));
        }
    }
}
