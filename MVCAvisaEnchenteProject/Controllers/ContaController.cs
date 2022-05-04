using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class ContaController : BaseController<Usuario, UsuarioDAO>
    {
        public ContaController()
        {
            DAOPrincipal = new UsuarioDAO();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro()
        {
            if (!UsuarioEstaLogado())
                return View(new RegistrarUsuarioViewModel());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(RegistrarUsuarioViewModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario(usuarioModel);
                    if (DAOPrincipal.EmailJaExiste(usuarioModel.Email))
                    {
                        TempData["Error"] = "Erro ao Cadastrar Usuário";
                        return View(usuarioModel);
                    }

                    DAOPrincipal.Inserir(usuario);

                    TempData["Success"] = "Usuário Cadastrado Com Sucesso!";
                    return RedirectToAction("Login");
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }

            return View(usuarioModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() 
        {
            if(!UsuarioEstaLogado())
                return View(new LoginUsuarioRequest());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginUsuarioRequest login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = DAOPrincipal.LogarUsuario(login);

                    if (usuario != null)
                    {
                        var userClaims = new List<Claim>()
                        {
                            new Claim("UsuarioId", usuario.Id.ToString()),
                            new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                            new Claim(ClaimTypes.Email, usuario.Email),
                            new Claim(ClaimTypes.Role, usuario.TipoUsuario.GetDescription())
                        };
                        var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                        var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                        HttpContext.SignInAsync(userPrincipal);

                        if (usuario.PrimeiroLogin)
                            return RedirectToAction("PrimeiroLogin");

                        return RedirectToAction("Index", "Home");
                    }

                    TempData["Error"] = "Falha no Login, Email ou Senha Incorretos!";
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }

            }

            return View(login);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PrimeiroLogin()
        {
            return View(new PrimeiroLoginViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult PrimeiroLogin(PrimeiroLoginViewModel primeiroLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = User.FindFirst("UsuarioId").Value;
                    DAOPrincipal.DefineCidadeUsuario(Convert.ToInt32(usuarioId), primeiroLogin.CidadeAtendidaId);
                    HttpContext.Session.SetString("PrimeiroLogin", "false");

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception erro)
                {
                    return View("Error", new ErrorViewModel(erro.ToString()));
                }
            }

            return View(primeiroLogin);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
