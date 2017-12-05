using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Services.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Iranum.Shop.PayOnDelivery.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nop.Plugin.Iranum.Shop.PayOnDelivery;

namespace Nop.Plugin.Iranum.Shop.PayOnDelivery.Controllers
{
    public class PayOnDeliveryController : BasePaymentController
    {
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly PayOnDeliveryPaymentSettings _ccAvenuePaymentSettings;
        private readonly PaymentSettings _paymentSettings;
        

        public PayOnDeliveryController(ISettingService settingService,
            IPaymentService paymentService, IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            PayOnDeliveryPaymentSettings ccAvenuePaymentSettings,
            PaymentSettings paymentSettings)
        {
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._ccAvenuePaymentSettings = ccAvenuePaymentSettings;
            this._paymentSettings = paymentSettings;
            
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel
            {
                MerchantId = _ccAvenuePaymentSettings.MerchantId,
                Key = _ccAvenuePaymentSettings.Key,
                MerchantParam = _ccAvenuePaymentSettings.MerchantParam,
                PayUri = _ccAvenuePaymentSettings.PayUri,
                AdditionalFee = _ccAvenuePaymentSettings.AdditionalFee,
                AccessCode = _ccAvenuePaymentSettings.AccessCode
            };

            return View("~/Plugins/Payments.CCAvenue/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _ccAvenuePaymentSettings.MerchantId = model.MerchantId;
            _ccAvenuePaymentSettings.Key = model.Key;
            _ccAvenuePaymentSettings.MerchantParam = model.MerchantParam;
            _ccAvenuePaymentSettings.PayUri = model.PayUri;
            _ccAvenuePaymentSettings.AdditionalFee = model.AdditionalFee;
            _ccAvenuePaymentSettings.AccessCode = model.AccessCode;
            _settingService.SaveSetting(_ccAvenuePaymentSettings);

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View("~/Plugins/Payment.Paystack/Views/PaymentInfo.cshtml");
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }

        [ValidateInput(false)]
        public ActionResult Return(FormCollection form)
        {
            string reference = Request.QueryString["reference"];
            string urlPath = "https://api.paystack.co/transaction/verify/" + reference;

            HttpClient verify = new HttpClient();
            verify.DefaultRequestHeaders.Clear();
            verify.DefaultRequestHeaders.Add("authorization", "Bearer " + "sk_test_61ebaf98bcc9f047c79f5dfe242dfae8ea43e861");
            verify.DefaultRequestHeaders.Add("accept", "application/json");
            verify.DefaultRequestHeaders.Add("cache-control", "no-cache");
            HttpResponseMessage resp = Task.Run(() => verify.GetAsync(urlPath)).Result;
            string stream = Task.Run(() => resp.Content.ReadAsStringAsync()).Result;

            dynamic data = JObject.Parse(stream);
            string status = data.data.status;
            //string reference_number = data.data.reference;

            var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(reference, "Payment.Paystack");

            if (status == "sauccess")
            {
                _orderProcessingService.MarkOrderAsPaid(order);
                return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
            }
            else
            {

                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [ValidateInput(false)]
        public ActionResult ReturnOLD(FormCollection form)
        {
            
            return Content("Security Error. Illegal access detected");
        }

        [AdminAuthorize]
        public ActionResult PayOnDeliveryAdmin()
        {
            ConfigurationModel config = new ConfigurationModel();
            config.Webhook = _ccAvenuePaymentSettings.Webhook;
            return View(config);
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult PayOnDeliveryAdmin(ConfigurationModel model)
        {
            _ccAvenuePaymentSettings.Webhook = model.Webhook;
            _settingService.SaveSetting(_ccAvenuePaymentSettings);

            return PayOnDeliveryAdmin();

        }
    }

}