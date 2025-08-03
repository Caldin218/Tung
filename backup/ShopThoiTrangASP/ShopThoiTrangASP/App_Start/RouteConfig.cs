using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopThoiTrangASP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "San-pham-theo-loai",
                url: "loai-san-pham/{slug}",
                defaults: new { controller = "Sanpham", action = "Category", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Trangchu",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Trangchu", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
 