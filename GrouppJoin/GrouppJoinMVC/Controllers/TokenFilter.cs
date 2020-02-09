using GroupJoinMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Controllers
{
    public class TokenFilterAttribute : TypeFilterAttribute
    {
        public TokenFilterAttribute() : base(typeof(TokenAuthFilter))
        {
        }
    }

    public class TokenAuthFilter : IAuthorizationFilter
    {
        AccountService service;
        public TokenAuthFilter(AccountService service)
        {
            this.service = service;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["X-Token"];
            if (!service.CheckAuthToken(token))
            {
                context.Result = new ForbidResult();
            }            
        }
    }
}
