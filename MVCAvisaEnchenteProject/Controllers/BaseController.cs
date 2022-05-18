using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public abstract class BaseController<T, D> : Controller where T : BaseEntity where D : BaseDAO<T>
    {
        public BaseController()
        {
            DAOPrincipal = Activator.CreateInstance(typeof(D)) as D;
        }

        protected D DAOPrincipal { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        public virtual IActionResult Index(int id)
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

        public virtual IActionResult Deletar(int id)
        {
            try
            {
                DAOPrincipal.Deletar(id);
                return Json(new JsonResponse());
            }
            catch (Exception erro)
            {
                return Json(new JsonResponse(messageErro: "Ocorreu um erro ao tentar Excluir!"));
            }
        }

        protected SelectList ObtemSelectListEstadosAtendidos()
        {
            var estadoDAO = new EstadoAtendidoDAO();
            var estados = estadoDAO.Listar();
            List<SelectListItem> selectEstados = new List<SelectListItem>();
            estados.ToList().ForEach(x =>
            {
                selectEstados.Add(new SelectListItem { Text = x.Descricao, Value = x.Id.ToString() });
            });

            return new SelectList(selectEstados, "Value", "Text");
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
