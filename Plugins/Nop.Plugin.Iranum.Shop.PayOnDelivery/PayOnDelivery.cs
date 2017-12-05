﻿using Nop.Core.Plugins;
using Nop.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using System.Web.Routing;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Configuration;

using Nop.Plugin.Iranum.Shop.PayOnDelivery.Controllers;
using Nop.Core.Domain.Payments;
using System.Web;
using Nop.Services.Directory;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Web.Framework;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Iranum.Shop.PayOnDelivery
{
    public class PayOnDelivery : BasePlugin, IPaymentMethod, IAdminMenuPlugin
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ISettingService _settingService;
        private readonly PayOnDeliveryPaymentSettings _manualPaymentSettings;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IOrderService _orderService;
        #endregion

        #region Ctor
        public PayOnDelivery(ILocalizationService localizationService,
            IOrderTotalCalculationService orderTotalCalculationService,
            ISettingService settingService, ICurrencyService currencyService, CurrencySettings currencySettings,
            PayOnDeliveryPaymentSettings manualPaymentSettings, IWebHelper webHelper, HttpContextBase httpContext, IOrderService orderService)
        {
            this._localizationService = localizationService;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._settingService = settingService;
            this._manualPaymentSettings = manualPaymentSettings;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._orderService = orderService;
        }
        #endregion

        #region Methods
        public override void Install()
        {
            //settings
            var settings = new PayOnDeliveryPaymentSettings()
            {
                MerchantId = "",
                Key = "",
                AccessCode = "",
                MerchantParam = "",

                
                PayUri = "https://api.paystack.co/transaction/initialize",
                AdditionalFee = 0,
                Webhook = "http://www.google.com"
            };

            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.RedirectionTip", "No further action is required from PayonDelivery Payment option, just confirm order in the next step ");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantId", "Merchant ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantId.Hint", "Enter merchant ID.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Key", "Working Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Key.Hint", "Enter working key.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantParam", "Merchant Param");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantParam.Hint", "Enter merchant param.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PayUri", "Pay URI");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PayUri.Hint", "Enter Pay URI.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AdditionalFee", "Additional fee");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AdditionalFee.Hint", "Enter additional fee to charge your customers.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AccessCode", "Access Code");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AccessCode.Hint", "Enter Access Code.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PaymentMethodDescription", "For payment you will be redirected to the Paystack website.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Webhook", "Webhook URL");

            base.Install();
        }

        public override void Uninstall()
        {

            //settings
            _settingService.DeleteSetting<PayOnDeliveryPaymentSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantId");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantId.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Key");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Key.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantParam");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.MerchantParam.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PayUri");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PayUri.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AdditionalFee.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AccessCode");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.AccessCode.Hint");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.PaymentMethodDescription");
            this.DeletePluginLocaleResource("Plugins.Iranum.Shop.PayOnDelivery.Webhook");
            base.Uninstall();
        }

        public string PaymentMethodDescription
        {
            //return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
            //for example, for a redirection payment method, description may be like this: "You will be redirected to PayPal site to complete the payment"
            get { return _localizationService.GetResource("Plugins.Iranum.Shop.PayOnDelivery.PaymentMethodDescription"); }
        }

        public PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.Redirection; }
        }

        public RecurringPaymentType RecurringPaymentType
        {
            get { return RecurringPaymentType.NotSupported; }
        }

        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        public bool SupportCapture
        {
            get { return false; }
        }

        public bool SupportPartiallyRefund
        {
            get { return false; }
        }

        public bool SupportRefund
        {
            get { return false; }
        }

        public bool SupportVoid
        {
            get { return false; }
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //CCAvenue is the redirection payment method
            //It also validates whether order is also paid (after redirection) so customers will not be able to pay twice

            //payment status should be Pending
            if (order.PaymentStatus != PaymentStatus.Pending)
                return false;

            //let's ensure that at least 1 minute passed after order is placed
            return !((DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes < 1);
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return _manualPaymentSettings.AdditionalFee;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "PaystackPayment";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Iranum.Shop.PayOnDelivery.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(PayOnDeliveryController);
        }

        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaystackPayment";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Iranum.Shop.PayOnDelivery.Controllers" }, { "area", null } };
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            HttpClient request = new HttpClient();
            request.DefaultRequestHeaders.Clear();



            this._webHelper.IsPostBeingDone = true;


            var order = postProcessPaymentRequest.Order;
            //_httpContext.Response.Redirect(_webHelper.GetStoreLocation());


            //return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
            var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current),
                               new RouteData());
            var urlHelper = new UrlHelper(context);
            var url = urlHelper.Action("Completed", "Checkout", new { orderId = order.Id });
            System.Web.HttpContext.Current.Response.Redirect(url);


        }

        //public async Task<string> GetAuthorizationUrl(string url, HttpContent content)
        //{

        //}

        private StringContent CreateContent(object model)
        {
            return new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
            );
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult { NewPaymentStatus = PaymentStatus.Pending };
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

       public void ManageSiteMap(Web.Framework.Menu.SiteMapNode rootNode)
       {
         var menuItem = new Web.Framework.Menu.SiteMapNode()
                {
                    SystemName = "PayOnDelivery",
                    Title = "PayOnDelivery Config",
                    ControllerName = "PayOnDeliveryPayment",
                    ActionName = "PayOnDeliveryAdmin",
                    Visible = true,
                    RouteValues = new RouteValueDictionary() { { "area", null } },
                };

                var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
                if (pluginNode != null)
                    pluginNode.ChildNodes.Add(menuItem);
                else
                    rootNode.ChildNodes.Add(menuItem);
            }
        }
        #endregion


    
}
