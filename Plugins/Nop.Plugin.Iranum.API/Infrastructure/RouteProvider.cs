using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Plugin.Iranum.API.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get { return 1; }
        }

        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            ViewEngines.Engines.Insert(0, new APIViewEngine());
        }
    }
}