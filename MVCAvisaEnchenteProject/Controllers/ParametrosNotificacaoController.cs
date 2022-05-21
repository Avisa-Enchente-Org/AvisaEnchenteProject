using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels.ParametrosNotificacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class ParametrosNotificacaoController : BaseController<ParametroNotificacao, ParametroNotificacaoDAO>
    {
        private readonly PontoDeSensoriamentoDAO _pontoSensoriamentoDAO;
        public ParametrosNotificacaoController()
        {
            _pontoSensoriamentoDAO = new PontoDeSensoriamentoDAO();
        }

        //public override IActionResult Index(int id)
        //{
        //    var pds = _pontoSensoriamentoDAO.ConsultarPorId(id);
        //    if (pds == null)
        //        return RedirectToAction("Index", "SensoriamentoAtual");

        //    var parametrosNotificacao = DAOPrincipal.ListarParametrosNotificacaoPorPDS(pds.Id);

        //    var indexViewModel = new ParametrosNotificaoIndexViewModel(pds, parametrosNotificacao);

        //    return View(indexViewModel);
        //}

        [HttpGet]
        public IActionResult CadastrarEditarParametros(int id)
        {
            var pds = _pontoSensoriamentoDAO.ConsultarPorId(id);
            if (pds == null)
                return RedirectToAction("Index", "SensoriamentoAtual");

            var parametrosNotificacao = DAOPrincipal.ListarParametrosNotificacaoPorPDS(id);
            if (!parametrosNotificacao.Any())
                return View(new CriarEditarParametrosNotificacaoViewModel(id));
                    

            return View(new CriarEditarParametrosNotificacaoViewModel(parametrosNotificacao));  
        }

        [HttpPost]
        public IActionResult SalvarParametrosNotificacao(CriarEditarParametrosNotificacaoViewModel model)
        {
            if (!ModelState.IsValid)
            {             
                TempData["Error"] = "Ocorreram erros de Cadastro, Verifique os campos dos parametros de notificação";
                return View("CadastrarEditarParametros", model);
            }


            return RedirectToAction("Index", "SensoriamentoAtual", new { id = model.PontoDeSensoriamentoId});
        }
    }
}
