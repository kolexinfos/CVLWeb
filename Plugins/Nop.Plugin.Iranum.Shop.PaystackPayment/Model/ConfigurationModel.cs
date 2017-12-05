using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Iranum.Shop.PaystackPayment.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.MerchantId")]
        public string MerchantId { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.Key")] //Encryption Key
        public string Key { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.MerchantParam")]
        public string MerchantParam { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.PayUri")] //Payment URI
        public string PayUri { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.AdditionalFee")]
        public decimal AdditionalFee { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.AccessCode")] //Access Code
        public string AccessCode { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PaystackPayment.Webhook")]
        public string Webhook { get; set; }

    }
}