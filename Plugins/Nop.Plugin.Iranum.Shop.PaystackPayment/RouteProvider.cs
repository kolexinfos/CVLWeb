using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Iranum.Shop.PaystackPayment
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //Return
            routes.MapRoute("Plugin.Iranum.Shop.PaystackPayment.Return",
                 "Plugins/PaystackPayment/Return",
                 new { controller = "PaystackPayment", action = "Return" },
                 new[] { "Nop.Plugin.Iranum.Shop.PaystackPayment.Controllers" }
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
