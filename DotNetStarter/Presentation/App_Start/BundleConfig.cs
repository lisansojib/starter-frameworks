using System.Web;
using System.Web.Optimization;

namespace Presentation
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                "~/App_Themes/home/css/material-font.css",
                "~/App_Themes/home/css/materialize.css",
                "~/App_Themes/home/css/style.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard").Include(
                      "~/Content/bootstrap.css"
                      , "~/Content/font-awesome.css"
                      , "~/App_Themes/adminlte/plugins/pace/theme-flash.css"
                      , "~/App_Themes/adminlte/plugins/bootstrap-table/bootstrap-table.css"
                      , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/auto-refresh/bootstrap-table-auto-refresh.css"
                      , "~/Content/toastr.css"
                      , "~/App_Themes/adminlte/plugins/sweetalert/sweet-alert.css"
                      //,"~/App_Themes/adminlte/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.css"
                      , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/filter-control/bootstrap-table-filter-control.css"
                      , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/fixed-columns/bootstrap-table-fixed-columns.css"
                      , "~/App_Themes/adminlte/plugins/bootstrap3-editable/css/bootstrap-editable.css"
                      , "~/App_Themes/adminlte/dist/css/AdminLTE.css"
                      , "~/App_Themes/adminlte/dist/css/skins/skin-grey.css"
                      , "~/App_Themes/adminlte/css/style.css"));

            bundles.Add(new ScriptBundle("~/Scripts/dashboard").Include(
                "~/App_Themes/adminlte/plugins/pace/pace.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/bootstrap-table.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/export/bootstrap-table-export.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/auto-refresh/bootstrap-table-auto-refresh.js"
                , "~/App_Themes/adminlte/plugins/tableexport.jquery.plugin/tableExport.min.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/filter-control/bootstrap-table-filter-control.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/fixed-columns/bootstrap-table-fixed-columns.js"
                , "~/App_Themes/adminlte/plugins/bootstrap3-editable/js/bootstrap-editable.js"
                , "~/App_Themes/adminlte/plugins/bootstrap-table/extensions/editable/bootstrap-table-editable.js"
                , "~/App_Themes/adminlte/plugins/jquery-slimscroll/jquery.slimscroll.js"
                , "~/App_Themes/adminlte/plugins/fastclick/fastclick.js"
                , "~/Scripts/toastr.js"
                //,"~/App_Themes/adminlte/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                , "~/Scripts/bootbox.js"
                , "~/App_Themes/adminlte/plugins/validatejs/validate.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/home").Include(
                "~/Scripts/jquery-3.3.1.js",
                "~/Content/themes/material/js/materialize.js",
                "~/Content/themes/material/js/init.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
