using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class PontoDeSensoriamentoController : BaseController<PontoDeSensoriamento, PontoDeSensoriamentoDAO>
    {
        public PontoDeSensoriamentoController() : base()
        {

        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Index()
        {
            return base.Index();
        }

    }
}
