using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public class PickUpStoreDeliveryListController : BasePublicController
    {
        // GET: PickUpStoreDeliveryList
        public ActionResult Index()
        {
            return View();
        }

        public PickUpStoreDeliveryListController()
        { }

        public virtual ActionResult OrderList(int? page)
        {
            return Json(JsonRequestBehavior.AllowGet) ;
        }
    }
}