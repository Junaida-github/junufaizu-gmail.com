using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace OnlineUsrToDoLst.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Scripts/vendor/bootstrap/css/bootstrap.min.css",
                       "~/Content/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                       "~/Content/fonts/Linearicons-Free-v1.0.0/icon-font.min.css",
                       "~/Scripts/vendor/animate/animate.css",
                       "~/Scripts/vendor/css-hamburgers/hamburgers.min.css",
                       "~/Scripts/vendor/select2/select2.min.css",
                       "~/Content/css/util.css",
                       "~/Content/css/main.css"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/vendor/jquery/jquery-3.2.1.min.js",
                        "~/Scripts/vendor/bootstrap/js/popper.js",
                        "~/Scripts/vendor/bootstrap/js/bootstrap.min.js",
                        "~/Scripts/js/main.js"
                        ));
        }
    }
}