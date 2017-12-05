using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CVLAPI.Controllers
{
    public class CategoryController : ApiController
    {
        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}