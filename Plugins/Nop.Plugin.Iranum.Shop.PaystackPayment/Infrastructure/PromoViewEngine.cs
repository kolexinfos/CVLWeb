using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.PromoSlider.Infrastructure
{
    public class PaystackViewEngine : ThemeableRazorViewEngine
    {
        public PaystackViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/Payment.Paystack/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/Payment.Paystack/Views/{0}.cshtml" };
        }
    }
}