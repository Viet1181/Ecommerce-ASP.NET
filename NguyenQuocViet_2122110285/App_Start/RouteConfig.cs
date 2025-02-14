using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace NguyenQuocViet_2122110285
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Bỏ qua các tệp tài nguyên (ví dụ: .axd)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Đăng ký route mặc định
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "NguyenQuocViet_2122110285.Controllers" } // Chỉ định namespace
            );
        }
    }
}

