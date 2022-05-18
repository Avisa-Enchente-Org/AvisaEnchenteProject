using Integracoes;
using Integracoes.Api;
using Integracoes.Models.Geocode;
using Integracoes.Models.Helix.PontoDeSensoriamento;
using Integracoes.Models.IBGE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps;
using MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class PontoDeSensoriamentoController : BaseController<PontoDeSensoriamento, PontoDeSensoriamentoDAO>
    {
        private readonly IntegracaoIBGE _integracaoIBGE;
        private readonly IntegracaoGeocode _integracaoGeocode;
        private readonly IntegracaoHelix _integracaoHelix;
        private readonly EstadoAtendidoDAO _estadoAtendidoDAO;
        private readonly CidadeAtendidaDAO _cidadeAtendidaDAO;

        public PontoDeSensoriamentoController() : base()
        {
            _integracaoIBGE = new IntegracaoIBGE(new IBGEApi());
            _integracaoGeocode = new IntegracaoGeocode(new GeoCodeApi());
            _integracaoHelix = new IntegracaoHelix(new HelixApi());
            _estadoAtendidoDAO = new EstadoAtendidoDAO();
            _cidadeAtendidaDAO = new CidadeAtendidaDAO();

        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Index(int id)
        {
            var indexViewModel = new IndexPontoDeSensoriamentoViewModel(DAOPrincipal.Listar());
            return View(indexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult CriarOuEditarPontoDeSensoriamento(int id = 0)
        {
            if (id == 0)
                return View(new CriarEditarPontoDeSensoriamentoViewModel());

            var pontoDeSensoriamento = DAOPrincipal.ConsultarPorId(id);
            if (pontoDeSensoriamento != null)
            {      
                return View(new CriarEditarPontoDeSensoriamentoViewModel(pontoDeSensoriamento));
            }

            TempData["Error"] = "Ponto de Sensoriamento não existe!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> SalvarPontoDeSensoriamento(int id, [Bind("Id, HelixId, Ativo, CodigoEstado, CodigoCidade, Latitude, Longitude")] CriarEditarPontoDeSensoriamentoViewModel pontoDeSensoriamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var transacao = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        #region Valida e Atualiza Informações da Cidade e Estado no Banco

                        var (jsonResponse, cidadeAtendidaId) = await AtualizaInfosEstadoECidade(pontoDeSensoriamentoViewModel.CodigoEstado,
                                                                                                pontoDeSensoriamentoViewModel.CodigoCidade,
                                                                                                pontoDeSensoriamentoViewModel.Latitude,
                                                                                                pontoDeSensoriamentoViewModel.Longitude);
                        if (jsonResponse.Erro)
                            return Json(jsonResponse);

                        #endregion


                        var pontoDeSensoriamento = new PontoDeSensoriamento(pontoDeSensoriamentoViewModel, cidadeAtendidaId: cidadeAtendidaId, usuarioId: Convert.ToInt32(ObterIdUsuarioLogado()));

                        var integracaoHelixResponse = await SalvaPontoDeSensoriamentoHelix(pontoDeSensoriamento: pontoDeSensoriamento);
                        if (integracaoHelixResponse.Erro)
                        {
                            return Json(integracaoHelixResponse);
                        }

                        if (id == 0)
                        {
                            DAOPrincipal.Inserir(pontoDeSensoriamento);
                        }
                        else
                        {
                            if (DAOPrincipal.ConsultarPorId(id) != null)
                                DAOPrincipal.Atualizar(pontoDeSensoriamento);
                            else
                                return Json(new JsonResponse(messageErro: "Ponto de Sensoriamento não encontrado!"));
                        }

                        transacao.Complete();
                    }
                    return Json(new JsonResponse(valido: true));
                }
                catch (Exception e)
                {
                    return Json(new JsonResponse(messageErro: "Ocorreu um erro ao tentar salvar o Ponto de Sensoriamento!"));
                }
            }
            return Json(new JsonResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "CriarOuEditarPontoDeSensoriamento", pontoDeSensoriamentoViewModel)));
        }

        [HttpPost]
        public async Task<JsonResponse> SalvaPontoDeSensoriamentoHelix(PontoDeSensoriamento pontoDeSensoriamento = null, int? id = null)
        {
            RestResponse helixUpsertResponse = null;
            if(string.IsNullOrEmpty(pontoDeSensoriamento?.HelixId) && id.HasValue)
            {
                pontoDeSensoriamento = DAOPrincipal.ConsultarPorId(id.Value);
            }

            var consultaEntidadeHelix = await _integracaoHelix.GetEntityById<PontoDeSensoriamentoHelixEntity>(pontoDeSensoriamento.HelixId);
            var pontoDeSensoriamentoHelixEntity = new PontoDeSensoriamentoHelixEntity(id: pontoDeSensoriamento.HelixId);

            if (pontoDeSensoriamento.Id == 0)
            {
                var existePDScomHelixId = DAOPrincipal.PontoDeSensoriamentoJaExiste(pontoDeSensoriamento.HelixId);
                if (consultaEntidadeHelix != null || existePDScomHelixId)
                    return new JsonResponse(messageErro: "Entidade já Cadastrada no Helix!");

            }
            if (consultaEntidadeHelix == null)
                helixUpsertResponse = await _integracaoHelix.CreateEntity<PontoDeSensoriamentoHelixEntity>(pontoDeSensoriamentoHelixEntity);


            if (helixUpsertResponse != null && helixUpsertResponse.IsSuccessful)
            {
                pontoDeSensoriamento.AtivoHelix = true;
                if (id.HasValue)
                    DAOPrincipal.Atualizar(pontoDeSensoriamento);

                return new JsonResponse(valido: true);
            }

            return new JsonResponse(valido: false);
        }

        private async Task<(JsonResponse JsonResponse, int CidadeAtendidaId)> AtualizaInfosEstadoECidade(string codigoEstado, string codigoCidade, string latitude, string longitude )
        {
            EstadoAtendido estadoAtendido;
            CidadeAtendida cidadeAtendida;
            int cidadeAtendidaId;

            try
            {
                var estadoSelecionado = await _integracaoIBGE.ObterEstadoPorCodigo(codigoEstado);
                var cidadeSelecionada = await _integracaoIBGE.ObterCidadePorCodigo(codigoCidade);

                var localizacaoValida = await ValidaLocalizacao(estadoSelecionado, cidadeSelecionada, latitude, longitude);
                if(localizacaoValida.Erro)
                    return (JsonResponse: localizacaoValida, CidadeAtendidaId: 0);


                estadoAtendido = _estadoAtendidoDAO.ConsultaEstadoPorCodigo(codigoEstado);
                cidadeAtendida = _cidadeAtendidaDAO.ConsultaCidadePorCodigo(codigoCidade);

                int estadoAtendidoId;
                if (estadoAtendido == null)
                {
                    estadoAtendido = new EstadoAtendido(estadoSelecionado.Nome, estadoSelecionado.Id.ToString(), estadoSelecionado.Sigla);
                    estadoAtendidoId = _estadoAtendidoDAO.ProximoId();
                    _estadoAtendidoDAO.Inserir(estadoAtendido);
                }
                else
                    estadoAtendidoId = estadoAtendido.Id;

                if (cidadeAtendida == null)
                {
                    cidadeAtendida = new CidadeAtendida(cidadeSelecionada.Nome, cidadeSelecionada.Id.ToString(), estadoAtendidoId);
                    cidadeAtendidaId = _cidadeAtendidaDAO.ProximoId();
                    _cidadeAtendidaDAO.Inserir(cidadeAtendida);
                }
                else
                    cidadeAtendidaId = cidadeAtendida.Id;


                return (JsonResponse: new JsonResponse(), CidadeAtendidaId: cidadeAtendidaId);
            }
            catch (Exception e)
            {
                return (JsonResponse: new JsonResponse(messageErro: "Ocorreu um erro ao tentar salvar a cidade ou estado!"), CidadeAtendidaId: 0);
            }
        }

        private async Task<JsonResponse> ValidaLocalizacao(Estado estadoSelecionado, Municipio cidadeSelecionada, string latitude, string longitude)
        {
            if (latitude.Contains(",") || longitude.Contains(","))
                return new JsonResponse(messageErro: "Latitude ou Longitude Inválidos, utilize o ponto (.) ao invés da virgula!");

            var validLatitude = decimal.TryParse(latitude.Replace(".", ","), out decimal latitudeDecimal);
            var validLongitude = decimal.TryParse(longitude.Replace(".", ","), out decimal longitudeDecimal);
            if(!validLatitude || !validLongitude)
                return new JsonResponse(messageErro: "Latitude ou Longitude Inválidos");

            var localizacaoLatLng = await _integracaoGeocode.ObtemLocalizacaoPorLatLng(latitude, longitude);

            if (localizacaoLatLng.status == Status.Ok)
            {
                var estadoLocalizado = localizacaoLatLng.results.Where(r => r.types.Contains(LocationType.Political) && r.address_components.Any(e => e.types.Contains(LocationType.EstadoType) && e.long_name == estadoSelecionado.Nome || e.short_name == estadoSelecionado.Sigla)).First();
                var cidadeLocalizada = localizacaoLatLng.results.Where(r => r.types.Contains(LocationType.Political) && r.address_components.Any(e => e.types.Contains(LocationType.CidadeType) && e.long_name == cidadeSelecionada.Nome || e.short_name == cidadeSelecionada.Nome)).First();

                if (estadoLocalizado == null || cidadeLocalizada == null)
                    return new JsonResponse(messageErro: "Erro, essa latitude e longitude não Coincidem com o Estado e Cidade Selecionados!");

                return new JsonResponse();
            }
            else
                return new JsonResponse(messageErro: "Erro, Latitude ou Longitude Inválidos!");
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult PesquisaAvancadaPontosDeSensoriamento(PesquisaAvancadaPontosDeSensoriamento pesquisaAvancadaPontosDeSensoriamento)
        {
            try
            {
                return Json(new JsonResponse(valido: true, html: HelperRenderRazorView.RenderRazorViewToString(this, "_ListarPontosDeSensoriamento", DAOPrincipal.PesquisaAvancadaPontosSensoriamento(pesquisaAvancadaPontosDeSensoriamento))));
            }
            catch (Exception e)
            {
                return Json(new JsonResponse(messageErro: "Ocorreu um erro ao pesquisar os Usuários!"));
            }
        }

    }
}
