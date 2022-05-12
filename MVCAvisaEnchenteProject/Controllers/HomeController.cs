using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCAvisaEnchenteProject.Infrastructure.CustomAttributes;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsuarioDAO _usuarioDAO;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _usuarioDAO = new UsuarioDAO();
        }

        [Route("/")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.User.FindFirst("UsuarioId")?.Value))
                return RedirectToAction("Index", "SensoriamentoAtual");

            return View();
        }

        public IActionResult SobreNos()
        {
            return View();
        }
        public IActionResult ComoFunciona()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult NotFoundPageView()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
