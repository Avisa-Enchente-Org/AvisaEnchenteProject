using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.GetTypedHeaders().Referer == null ||
                context.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != context.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
    }
}
