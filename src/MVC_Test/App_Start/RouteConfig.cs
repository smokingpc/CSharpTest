using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Test
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //這邊 defaults: new {controller= "Route", action = "Index"}
            //"Route" 會自動後綴 "Controller" ，然後去找名為 "RouteController" 的controller class 
            //"Index" 是 controller裡面註冊的ActionMethod，兩個合起來就是 "呼叫 RouteController::Index()" 的意思
            //但這邊controller名稱若變更，view的目錄名稱也要跟著變...例如
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Test", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
