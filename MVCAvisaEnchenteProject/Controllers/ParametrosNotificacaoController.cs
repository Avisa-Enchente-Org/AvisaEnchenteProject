using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.ParametrosNotificacao;
using MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace MVCAvisaEnchenteProject.Controllers
{
    [Authorize(Roles = nameof(ETipoUsuario.Admin))]
    public class ParametrosNotificacaoController : BaseController<ParametroNotificacao, ParametroNotificacaoDAO>
    {
        private readonly PontoDeSensoriamentoDAO _pontoSensoriamentoDAO;
        private readonly RegistroSensoriamentoDAO _registroSensoriamentoDAO;
        public ParametrosNotificacaoController()
        {
            _pontoSensoriamentoDAO = new PontoDeSensoriamentoDAO();
            _registroSensoriamentoDAO = new RegistroSensoriamentoDAO();
        }

        [HttpGet]
        public override IActionResult Index(int id)
        {
            try
            {
                return View(new PesquisaAvancadaAlertasRiscoViewModel());
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        [HttpGet]
        public IActionResult CadastrarEditarParametros(int id)
        {
            var pds = _pontoSensoriamentoDAO.ConsultarPorId(id);
            if (pds == null)
                return RedirectToAction("Index", "SensoriamentoAtual");

            var parametrosNotificacao = DAOPrincipal.ListarParametrosNotificacaoPorPDS(id);
            if (!parametrosNotificacao.Any())
                return View(new CriarEditarParametrosNotificacaoViewModel(id));
                    

            return View(new CriarEditarParametrosNotificacaoViewModel(parametrosNotificacao, id));  
        }

        [HttpPost]
        public IActionResult SalvarParametrosNotificacao(CriarEditarParametrosNotificacaoViewModel model)
        {
            if (!ModelState.IsValid)
            {             
                TempData["Error"] = "Ocorreram erros de Cadastro, Verifique os campos dos parametros de notificação";
                return View("CadastrarEditarParametros", model);
            }
            try
            {
                using (var transacao = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var param in model.ParametrosNotificacaoModel)
                    {
                        if(param.Id > 0)
                            DAOPrincipal.Atualizar(new ParametroNotificacao(param));
                        else
                            DAOPrincipal.Inserir(new ParametroNotificacao(param));
                    }
                    transacao.Complete();
                }

                return RedirectToAction("Detalhes", "PontoDeSensoriamento", new { id = model.PontoDeSensoriamentoId });
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocorreram erros internos, Tente novamente mais tarde!";
                return View("CadastrarEditarParametros", model);
            }
        }

        public override IActionResult Deletar(int id)
        {
            try
            {
                var pds = _pontoSensoriamentoDAO.ConsultarPorId(id);
                if (pds == null)
                    return RedirectToAction("Index", "SensoriamentoAtual");

                var parametrosNotificacao = DAOPrincipal.ListarParametrosNotificacaoPorPDS(id);
                if (!parametrosNotificacao.Any())
                    return RedirectToAction("Detalhes", "PontoDeSensoriamento", new { id = id });

                using (var transacao = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var param in parametrosNotificacao)
                    {
                        DAOPrincipal.Deletar(param.Id);
                    }
                    transacao.Complete();
                }

                return RedirectToAction("Detalhes", "PontoDeSensoriamento", new { id = id });
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocorreram erros internos, Tente novamente mais tarde!";
                return RedirectToAction("Detalhes", "PontoDeSensoriamento", new { id = id });
            }
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult PesquisaAvancadaAlertasDeRisco(PesquisaAvancadaAlertasRiscoViewModel pesquisaAvancadaPontosDeSensoriamento)
        {
            try
            {
                return Json(new JsonResponse(valido: true, html: HelperRenderRazorView.RenderRazorViewToString(this, "_ListarAlertasRisco", _registroSensoriamentoDAO.PesquisaAvancadaAlertasRisco(pesquisaAvancadaPontosDeSensoriamento))));
            }
            catch (Exception e)
            {
                return Json(new JsonResponse(messageErro: "Ocorreu um erro ao pesquisar os Alertas de Risco!"));
            }
        }
    }
}
