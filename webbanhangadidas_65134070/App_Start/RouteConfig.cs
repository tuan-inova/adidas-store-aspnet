using System.Web.Mvc;
using System.Web.Routing;

namespace webbanhangadidas_65134070
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Bỏ qua route cho tài nguyên hệ thống
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // =========================
            // ROUTE CHO ADMIN
            // =========================
            routes.MapRoute(
                name: "Admin",
                url: "admin/{action}/{id}",
                defaults: new
                {
                    controller = "SanPhamAdmin_65134070",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            // =========================
            // ROUTE MẶC ĐỊNH (TRANG CHỦ)
            // =========================
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}
