using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KRA.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Parameters", action = "ManageParameterDefinations", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "PMReportExcel",
             url: "{controller}/{action}/{id}/{year}",
             defaults: new { controller = "KraScores", action = "GetPMReport", id = UrlParameter.Optional, year = UrlParameter.Optional }
             
             
         );

            routes.MapRoute(
             name: "ADMReportExcel",
             url: "{controller}/{action}/{id}/{year}",
             defaults: new { controller = "KraScores", action = "GetADMReportExcel", id = UrlParameter.Optional, year = UrlParameter.Optional }
         );
            
        }
    }
}
