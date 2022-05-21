using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.CustomAttributes;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps;
using MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    [Authorize]
    public class SensoriamentoAtualController : BaseController<RegistroSensoriamento, RegistroSensoriamentoDAO>
    {
        private readonly UsuarioDAO _usuarioDAO;
        private readonly PontoDeSensoriamentoDAO _pdsDAO;

        public SensoriamentoAtualController()
        {
            _usuarioDAO = new UsuarioDAO();
            _pdsDAO = new PontoDeSensoriamentoDAO();
        }

        [RequiredFirstAccessConfig]
        public override IActionResult Index(int id)
        {
            var usuarioEnderecoLogado = _usuarioDAO.ConsultarEnderecoUsuario(Convert.ToInt32(ObterIdUsuarioLogado()));
            if (usuarioEnderecoLogado != null)
            {
                var model = new IndexSensoriamentoAtualViewModel(usuarioEnderecoLogado);
                return View(model);
            }

            return View(new IndexSensoriamentoAtualViewModel());
        }

        [RequiredFirstAccessConfig]
        public IActionResult DashboardSensoriamento(int id)
        {
            var pontoDeSensoriamentoAtual = DAOPrincipal.ConsultarPorPontoDeSensoriamentoId(id);
            var pontoDeSensoriamento = _pdsDAO.ConsultarPorId(pontoDeSensoriamentoAtual.PontoDeSensoriamentoId);

            if(pontoDeSensoriamentoAtual != null)
                return View(new DashboardViewModel(pontoDeSensoriamentoAtual, $"{pontoDeSensoriamento.CidadeAtendida.Descricao}, {pontoDeSensoriamento.EstadoAtendido.UF}"));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ObtemSensoriamentosAtuaisPorCidade(int cidadeAtendidaId)
        {
            var pontosDeSensoriamentosMarkers = new List<PontosDeSensoriamentoMapMarkers>();
            var pontosDeSensoriamentoAtuais = DAOPrincipal.ListarSensoriamentoAtualPorCidade(cidadeAtendidaId);

            pontosDeSensoriamentosMarkers.AddRange(pontosDeSensoriamentoAtuais.Select(p => new PontosDeSensoriamentoMapMarkers(p.PontoDeSensoriamento.Id, p.PontoDeSensoriamento.Latitude, p.PontoDeSensoriamento.Longitude, (int)p.TipoRisco)));

            return Json(pontosDeSensoriamentosMarkers);
        }

        [HttpGet]
        public IActionResult ObtemSensoriamentoAtual(int pontoDeSensoriamentoId)
        {
            var pontoDeSensoriamentoAtual = DAOPrincipal.ConsultarPorPontoDeSensoriamentoId(pontoDeSensoriamentoId);
            if(pontoDeSensoriamentoAtual != null)
                return Json(new { notFound = false, data = pontoDeSensoriamentoAtual });

            return Json(new { notFound = true });
        }

        [HttpGet]
        public IActionResult AtualizaGoogleMapsComponent()
        {
            return ViewComponent("GoogleMaps", _usuarioDAO.ConsultarEnderecoUsuario(Convert.ToInt32(ObterIdUsuarioLogado())));
        }

        public override IActionResult Deletar(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
