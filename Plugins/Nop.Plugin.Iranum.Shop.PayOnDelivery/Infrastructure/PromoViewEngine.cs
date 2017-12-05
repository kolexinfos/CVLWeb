using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.PromoSlider.Infrastructure
{
    public class PayOnDeliveryViewEngine : ThemeableRazorViewEngine
    {
        public PayOnDeliveryViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/Payment.PayOnDelivery/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/Payment.PayOnDelivery/Views/{0}.cshtml" };
        }
    }
}