using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVLAPI.Controllers
{
    public class CustomerController : ApiController
    {
        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.Route("")]
        public IHttpActionResult Get()
        {
            Customer customer = new Customer();
            //var registerRequest = new CustomerRegistrationRequest(customer,)
            return Ok();
        }
    }
}
