using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Iranum.API.Infrastructure
{
    public class APIViewEngine : ThemeableRazorViewEngine
    {
        public APIViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/Iranum.API/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/Iranum.API/Views/{0}.cshtml" };
        }
    }
}