using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Iranum.Shop.PaystackPayment
{
    public class PaystackPaymentSettings : ISettings
    {
        public string MerchantId { get; set; }
        public string Key { get; set; }
        public string MerchantParam { get; set; }
        public string PayUri { get; set; }
        public decimal AdditionalFee { get; set; }
        public string AccessCode { get; set; }
        public string Webhook { get; set; }
    }
}
