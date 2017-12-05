using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Iranum.Shop.PayOnDelivery.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.MerchantId")]
        public string MerchantId { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.Key")] //Encryption Key
        public string Key { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.MerchantParam")]
        public string MerchantParam { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.PayUri")] //Payment URI
        public string PayUri { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.AdditionalFee")]
        public decimal AdditionalFee { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.AccessCode")] //Access Code
        public string AccessCode { get; set; }

        [NopResourceDisplayName("Plugins.Iranum.Shop.PayOnDelivery.Webhook")]
        public string Webhook { get; set; }
    }
}