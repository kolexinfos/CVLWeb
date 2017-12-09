using Nop.Services.Configuration;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Iranum.API.Controllers
{
    class CvlapiController : BaseController
    {
        private readonly ISettingService _settingService;
        
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        
        

        public CvlapiController(ISettingService settingService,
            IOrderService orderService,IOrderProcessingService orderProcessingService)
        {
            this._settingService = settingService;
            
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            
        }

        [ValidateInput(false)]
        public ActionResult Return(FormCollection form)
        {
            return new JsonResult();
        }

    }
}
