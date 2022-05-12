using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.CustomAttributes;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    [Authorize]
    public class SensoriamentoAtualController : BaseController<SensoriamentoAtual, SensoriamentoAtualDAO>
    {
        private readonly UsuarioDAO _usuarioDAO;

        public SensoriamentoAtualController()
        {
            _usuarioDAO = new UsuarioDAO();
        }

        [RequiredFirstAccessConfig]
        public override IActionResult Index()
        {
            var usuarioEnderecoLogado = _usuarioDAO.ConsultarEnderecoUsuario(Convert.ToInt32(ObterIdUsuarioLogado()));
            if (usuarioEnderecoLogado != null)
            {
                var enderecoUsuario = $"{usuarioEnderecoLogado.CidadeAtendida.Descricao}, {usuarioEnderecoLogado.EstadoAtendido.Descricao}";

                var pontosDeSensoriamento = DAOPrincipal.ListarSensoriamentoAtualPorCidade(usuarioEnderecoLogado.CidadeAtendida.Id);

                var mapPointCenter = new GoogleMapsModel(enderecoUsuario, pontosDeSensoriamento, usuarioEnderecoLogado.CidadeAtendida.Id);
                return View(mapPointCenter);
            }

            return View(new GoogleMapsModel());
        }

        [HttpPost]
        public async Task<ActionResult> ObtemSensoriamentosAtuaisPorCidade(int cidadeAtendidaId)
        {
            var pontosDeSensoriamentosMarkers = new List<PontosDeSensoriamentoMapMarkers>();
            var pontosDeSensoriamentoAtuais = DAOPrincipal.ListarSensoriamentoAtualPorCidade(cidadeAtendidaId);

            pontosDeSensoriamentosMarkers.AddRange(pontosDeSensoriamentoAtuais.Select(p => new PontosDeSensoriamentoMapMarkers(p.PontoDeSensoriamento.Id, p.PontoDeSensoriamento.Latitude, p.PontoDeSensoriamento.Longitude, (int)p.TipoRisco)));

            return Json(pontosDeSensoriamentosMarkers);
        }

        public override IActionResult Deletar(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
