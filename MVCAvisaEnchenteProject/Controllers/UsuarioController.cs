using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class UsuarioController : BaseController<Usuario, UsuarioDAO>
    {
        public UsuarioController()
        {
            DAOPrincipal = new UsuarioDAO();
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Index()
        {
            return base.Index();
        }

        [HttpGet]
        [Authorize]
        public IActionResult MeuPerfil()
        {
            return View(ObterUsuarioLogado() ?? new Usuario());
        }
    }
}
