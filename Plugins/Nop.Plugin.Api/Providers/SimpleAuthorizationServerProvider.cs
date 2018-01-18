using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        public SimpleAuthorizationServerProvider(ICustomerService customerService , ICustomerRegistrationService customerRegistrationService)
        {
            _customerRegistrationService = customerRegistrationService;
            _customerService = customerService;
            
        }

        //public SimpleAuthorizationServerProvider()
        //{

        //}
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            //    if (user == null)
            //    {
            //        context.SetError("invalid_grant", "The user name or password is incorrect.");
            //        return;
            //    }
            //}

            var loginResult = _customerRegistrationService.ValidateCustomer(context.UserName.Trim(), context.Password);

            switch(loginResult)
            {
                case CustomerLoginResults.Successful:
                    {
                        Customer customer = _customerService.GetCustomerByEmail(context.UserName);
                        CustomerRole role = _customerService.GetCustomerRoleById(customer.Id);



                        //https://www.nopcommerce.com/boards/t/21617/how-to-check-if-the-user-has-a-role.aspx
                        if (customer.IsVendor())
                        {
                            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                            identity.AddClaim(new Claim("sub", context.UserName));
                            identity.AddClaim(new Claim("role", "user"));

                        var props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            { "vendor_id" , customer.VendorId.ToString() }

                        });

                        var ticket = new AuthenticationTicket(identity, props);
                        context.Validated(ticket);

                        //context.Validated(identity);
                        return;
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Account.Login.WrongCredentials.NotVendor");
                            return;
                        }

                    }
                case CustomerLoginResults.CustomerNotExist:
                    
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials.CustomerNotExist");
                    return;
                case CustomerLoginResults.Deleted:
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials.Deleted");
                    return;
                case CustomerLoginResults.NotActive:
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials.NotActive");
                    return;
                case CustomerLoginResults.NotRegistered:
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials.NotRegistered");
                    return;
                case CustomerLoginResults.LockedOut:
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials.LockedOut");
                    return;
                case CustomerLoginResults.WrongPassword:
                default:
                    context.SetError("invalid_grant", "Account.Login.WrongCredentials");
                    return;
            }

            

        }
    }
}
