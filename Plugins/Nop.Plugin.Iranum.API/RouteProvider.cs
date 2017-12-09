using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Iranum.API
{
   public partial class RouteProvider: IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //Return
            //routes.MapRoute("Plugin.Iranum.API.Return",
            //     "Plugins/Cvlapi/Return",
            //     new { controller = "Cvlapi", action = "Return" },
            //     new[] { "Nop.Plugin.Iranum.API.Controllers" }
            //);

            RouteCollectionExtensions.MapRoute(routes, "Plugin.Iranum.API.Return",
                 "Plugins/API/Return",
                 new { controller = "Cvlapi", action = "Return" },
                 new string[1]{ "Nop.Plugin.Iranum.API.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
