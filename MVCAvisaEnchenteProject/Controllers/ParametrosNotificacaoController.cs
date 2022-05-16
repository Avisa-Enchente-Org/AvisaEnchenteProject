using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class ParametrosNotificacaoController : : BaseController<ParametroNotificacao, ParametroNotificacaoDAO>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
