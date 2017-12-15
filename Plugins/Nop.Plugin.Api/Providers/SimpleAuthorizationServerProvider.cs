﻿using Microsoft.Owin.Security.OAuth;
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

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

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
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("sub", context.UserName));
                        identity.AddClaim(new Claim("role", "user"));

                        context.Validated(identity);
                        return;
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
