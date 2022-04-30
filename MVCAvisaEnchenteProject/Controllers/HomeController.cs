using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsuarioDAO _usuarioDAO;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _usuarioDAO = new UsuarioDAO();
        }

        
        public IActionResult Index()
        {
            if(VerificaSePrimeiroLogin())
                return RedirectToAction("PrimeiroLogin", "Conta");

            return View();
        }
        public IActionResult SobreNos()
        {
            if (VerificaSePrimeiroLogin())
                return RedirectToAction("PrimeiroLogin", "Conta");

            return View();
        }
        public IActionResult ComoFunciona()
        {
            if (VerificaSePrimeiroLogin())
                return RedirectToAction("PrimeiroLogin", "Conta");

            return View();
        }

        public IActionResult NotFoundPageView()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool VerificaSePrimeiroLogin()
        {
            var usuarioId = User?.FindFirst("UsuarioId")?.Value;
            if (!string.IsNullOrEmpty(usuarioId))
            {
                if (_usuarioDAO.ConsultarPorId(Convert.ToInt32(usuarioId)).PrimeiroLogin)
                {
                    TempData["Info"] = "Primeiro Selecione uma Localização para acessar outras páginas do Site";
                    return true;
                }
            }

            return false;
        }
    }
}
