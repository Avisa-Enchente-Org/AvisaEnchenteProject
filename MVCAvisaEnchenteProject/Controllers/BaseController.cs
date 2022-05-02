using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public abstract class BaseController<T, D> : Controller where T : BaseEntity where D : BaseDAO<T>
    {
        protected D DAOPrincipal { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "form";

        public virtual IActionResult Index()
        {
            try
            {
                var lista = DAOPrincipal.Listar();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Deletar(int id)
        {
            try
            {
                DAOPrincipal.Deletar(id);
                return Json(new JsonFormResponse());
            }
            catch (Exception erro)
            {
                return Json(new JsonFormResponse(messageErro: "Ocorreu um erro ao tentar Excluir!"));
            }
        }

        protected bool UsuarioEstaLogado() => !string.IsNullOrEmpty(User?.FindFirst("UsuarioId")?.Value);
        protected string ObterIdUsuarioLogado() => User?.FindFirst("UsuarioId")?.Value;
        protected Usuario ObterUsuarioLogado()
        {
            var usuarioDAO = new UsuarioDAO();
            if (UsuarioEstaLogado())
                return usuarioDAO.ConsultarPorId(Convert.ToInt32(ObterIdUsuarioLogado()));

            return null;
        }
    }
}
