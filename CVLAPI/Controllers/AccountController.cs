using CVLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CVLAPI.Controllers
{
    public class AccountController : ApiController
    {
        public AccountController()
        {
           
        }
        

        // POST api/Account/Register
        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Mvc.Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult()
        {
            
            return null;
        }
    }
}