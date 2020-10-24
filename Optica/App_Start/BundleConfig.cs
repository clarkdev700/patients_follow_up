using System.Web;
using System.Web.Optimization;

namespace Optica
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //declare my bundles
            //--- Styles part
            bundles.Add(new StyleBundle("~/AppTheme/firstPartStyle").Include(
                "~/AppTheme/bootstrap/css/bootstrap.min.css",
                "~/AppTheme/awesome/css/font-awesome.min.css",
                "~/AppTheme/dist/css/AdminLTE.css",
                "~/AppTheme/plugins/iCheck/square/blue.css"
                ));
            bundles.Add(new StyleBundle("~/AppTheme/SecondStyle").Include(
                "~/AppTheme/dist/css/skins/_all-skins.min.css",
                "~/AppTheme/plugins/iCheck/flat/blue.css",
                "~/AppTheme/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jqxStyle").Include(
                "~/Content/jqx.base.css",
                "~/Content/jqx.darkblue.css"
                ));
            //--- Scripts part
            bundles.Add(new ScriptBundle("~/AppTheme/firstPartScripts").Include(
                "~/AppTheme/plugins/jQuery/jQuery-2.1.3.min.js",
                "~/AppTheme/bootstrap/js/bootstrap.min.js",
                "~/AppTheme/plugins/iCheck/icheck.min.js"
                ));
            bundles.Add(new ScriptBundle("~/AppTheme/secondPartScripts").Include(
                "~/AppTheme/plugins/jQueryUI/jquery-ui-1.10.3.min.js",
                "~/AppTheme/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                "~/AppTheme/plugins/slimScroll/jquery.slimscroll.min.js",
                "~/AppTheme/plugins/fastclick/fastclick.min.js",
                "~/AppTheme/dist/js/app.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/jqxBase").Include(
                    "~/Scripts/jqxcore.js",
                    "~/Scripts/jqxdata.js",
                    "~/Scripts/jqxbuttons.js",
                    "~/Scripts/jqxmenu.js",
                    "~/Scripts/jqxscrollbar.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/jqxTable").Include(
                    "~/Scripts/jqxlistbox.js",
                    "~/Scripts/jqxdropdownlist.js",
                    "~/Scripts/jqxdatatable.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/jqxGrid").Include(
                    "~/Scripts/jqxgrid.js",
                    "~/Scripts/jqxgrid.selection.js",
                    "~/Scripts/jqxgrid.columnsresize.js",
                    "~/Scripts/jqxgrid.filter.js",
                    "~/Scripts/jqxgrid.sort.js",
                    "~/Scripts/jqxgrid.pager.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/jqxChart").Include(
                "~/Scripts/jqxdraw.js",
                "~/Scripts/jqxchart.core.js"
                ));
        }
    }
}
