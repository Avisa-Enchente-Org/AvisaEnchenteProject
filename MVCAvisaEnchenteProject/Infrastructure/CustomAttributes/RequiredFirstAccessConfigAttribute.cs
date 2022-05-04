using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiredFirstAccessConfigAttribute : ActionFilterAttribute
    {
        private readonly UsuarioDAO _usuarioDAO;
        public RequiredFirstAccessConfigAttribute()
        {
            _usuarioDAO = new UsuarioDAO();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var primeiroLogin = context.HttpContext.Session.GetString("PrimeiroLogin");
            if (!string.IsNullOrEmpty(primeiroLogin))
                VerificaPrimeiroLogin(context, context.HttpContext.Session.GetString("PrimeiroLogin"));           
            else
            {
                var userId = context.HttpContext.User.FindFirst("UsuarioId").Value;
                context.HttpContext.Session.SetString("PrimeiroLogin", !string.IsNullOrEmpty(userId) ? _usuarioDAO.ConsultarPorId(Convert.ToInt32(userId)).PrimeiroLogin.ToString().ToLower() : "false");
                VerificaPrimeiroLogin(context, context.HttpContext.Session.GetString("PrimeiroLogin"));
            }
        }

        private void VerificaPrimeiroLogin(ActionExecutingContext context, string primeiroLogin)
        {
            if (primeiroLogin.ToLower() == true.ToString().ToLower())
                context.HttpContext.Response.Redirect("/Conta/PrimeiroLogin");

            base.OnActionExecuting(context);
        }

    }
}
