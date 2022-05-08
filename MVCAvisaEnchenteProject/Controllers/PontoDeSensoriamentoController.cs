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
using MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class PontoDeSensoriamentoController : BaseController<PontoDeSensoriamento, PontoDeSensoriamentoDAO>
    {
        private readonly IntegracaoIBGE _integracaoIBGE;
        private readonly IntegracaoGeocode _integracaoGeocode;
        private readonly IntegracaoHelix _integracaoHelix;
        private readonly EstadoAtendidoDAO _estadoAtendidoDAO;
        private readonly CidadeAtendidaDAO _cidadeAtendidaDAO;
        private readonly UsuarioDAO _usuarioDAO;
        public PontoDeSensoriamentoController() : base()
        {
            _integracaoIBGE = new IntegracaoIBGE(new IBGEApi());
            _integracaoGeocode = new IntegracaoGeocode(new GeoCodeApi());
            _integracaoHelix = new IntegracaoHelix(new HelixApi());
            _estadoAtendidoDAO = new EstadoAtendidoDAO();
            _cidadeAtendidaDAO = new CidadeAtendidaDAO();
            _usuarioDAO = new UsuarioDAO();
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Index()
        {
            var indexViewModel = new IndexPontoDeSensoriamentoViewModel(ObtemSelectListEstadosAtendidos(), ObtemSelectListUsuariosAdmin(), DAOPrincipal.Listar());
            return View(indexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> CriarOuEditarPontoDeSensoriamento(int id = 0)
        {
            if (id == 0)
                return View(new CriarEditarPontoDeSensoriamentoViewModel(await ObtemSelectListEstados()));

            var pontoDeSensoriamento = DAOPrincipal.ConsultarPorId(id);
            if (pontoDeSensoriamento != null)
            {      
                return View(new CriarEditarPontoDeSensoriamentoViewModel(pontoDeSensoriamento, await ObtemSelectListEstados(pontoDeSensoriamento.EstadoAtendido.CodigoEstado)));
            }

            TempData["Error"] = "Ponto de Sensoriamento não existe!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> SalvarPontoDeSensoriamento(int id, [Bind("Id, HelixId, Ativo, CodigoEstado, CodigoCidade, Latitude, Longitude")] CriarEditarPontoDeSensoriamentoViewModel pontoDeSensoriamentoViewModel)
        {
            pontoDeSensoriamentoViewModel.Estados = await ObtemSelectListEstados(pontoDeSensoriamentoViewModel.CodigoEstado);
            if (ModelState.IsValid)
            {
                #region Valida e Atualiza Informações da Cidade e Estado no Banco

                var (jsonResponse, cidadeAtendidaId) = await AtualizaInfosEstadoECidade(pontoDeSensoriamentoViewModel.CodigoEstado, 
                                                                                        pontoDeSensoriamentoViewModel.CodigoCidade, 
                                                                                        pontoDeSensoriamentoViewModel.Latitude, 
                                                                                        pontoDeSensoriamentoViewModel.Longitude);
                if (jsonResponse.Erro)
                    return Json(jsonResponse);

                #endregion

                try
                {
                    var pontoDeSensoriamento = new PontoDeSensoriamento(pontoDeSensoriamentoViewModel, cidadeAtendidaId: cidadeAtendidaId, usuarioId: Convert.ToInt32(ObterIdUsuarioLogado()));

                    var integracaoHelixResponse = await SalvaPontoDeSensoriamentoHelix(pontoDeSensoriamento: pontoDeSensoriamento);
                    if (integracaoHelixResponse.Erro)
                    {
                        _cidadeAtendidaDAO.Deletar(cidadeAtendidaId);
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

                    return Json(new JsonResponse(valido: true));
                }
                catch (Exception e)
                {
                    _cidadeAtendidaDAO.Deletar(cidadeAtendidaId);
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
                if (consultaEntidadeHelix != null)
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
            int cidadeAtendidaId = default;

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
                    cidadeAtendida = new CidadeAtendida(cidadeSelecionada.Nome, cidadeSelecionada.Id.ToString(), estadoAtendidoId, -23.8046745M, -46.6718363M);
                    cidadeAtendidaId = _cidadeAtendidaDAO.ProximoId();
                    _cidadeAtendidaDAO.Inserir(cidadeAtendida);
                }
                else
                    cidadeAtendidaId = cidadeAtendida.Id;


                return (JsonResponse: new JsonResponse(), CidadeAtendidaId: cidadeAtendidaId);
            }
            catch (Exception e)
            {
                _cidadeAtendidaDAO.Deletar(cidadeAtendidaId);
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
                var estadoLocalizado = localizacaoLatLng.results.Any(r => r.types.Contains(LocationType.Political) && r.address_components.Any(e => e.types.Contains(LocationType.EstadoType) && e.long_name == estadoSelecionado.Nome || e.short_name == estadoSelecionado.Sigla));
                var cidadeLocalizada = localizacaoLatLng.results.Any(r => r.types.Contains(LocationType.Political) && r.address_components.Any(e => e.types.Contains(LocationType.CidadeType) && e.long_name == cidadeSelecionada.Nome || e.short_name == cidadeSelecionada.Nome));

                if (!estadoLocalizado || !cidadeLocalizada)
                    return new JsonResponse(messageErro: "Erro, essa latitude e longitude não Coincidem com o Estado e Cidade Selecionados!");

                return new JsonResponse();
            }
            else
                return new JsonResponse(messageErro: "Erro, Latitude ou Longitude Inválidos!");
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> PesquisaAvancadaPontosDeSensoriamento(PesquisaAvancadaPontosDeSensoriamento pesquisaAvancadaPontosDeSensoriamento)
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

        private async Task<SelectList> ObtemSelectListEstados(string codigoEstado = null)
        {
            var estados = await _integracaoIBGE.ListarEstados();
            List<SelectListItem> selectEstados = new List<SelectListItem>();
            estados.ToList().ForEach(x =>
            {
                selectEstados.Add(new SelectListItem { Text = x.Nome, Value = x.Id.ToString() });
            });

            return new SelectList(selectEstados, "Value", "Text", codigoEstado);
        }

        [HttpPost]
        public async Task<ActionResult> ObtemSelectListCidadesPorUF(string uf)
        {
            List<SelectListItem> selectCidades = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(uf))
            {
                var cidades = await _integracaoIBGE.ListarCidades(uf);
                cidades.ToList().ForEach(x =>
                {
                    selectCidades.Add(new SelectListItem { Text = x.Nome, Value = x.Id.ToString() });
                });

            }
            return Json(selectCidades);
        }


        [HttpPost]
        public async Task<ActionResult> ObtemSelectListCidadesAtendidas(int estadoId)
        {
            List<SelectListItem> selectCidades = new List<SelectListItem>();

            var cidades = _cidadeAtendidaDAO.ListarCidadesAtendidasPorEstadoId(estadoId);
            cidades.ToList().ForEach(x =>
            {
                selectCidades.Add(new SelectListItem { Text = x.Descricao, Value = x.Id.ToString() });
            });
     
            return Json(selectCidades);
        }

        private SelectList ObtemSelectListUsuariosAdmin()
        {
            var usuarios = _usuarioDAO.ListarUsuariosAdministradores();
            List<SelectListItem> selectUsuarios = new List<SelectListItem>();
            usuarios.ToList().ForEach(x =>
            {
                selectUsuarios.Add(new SelectListItem { Text = x.NomeCompleto, Value = x.Id.ToString() });
            });

            return new SelectList(selectUsuarios, "Value", "Text");
        }

        public override IActionResult Deletar(int id)
        {
            var helixId = DAOPrincipal.ConsultarPorId(id).HelixId;
            _integracaoHelix.DeleteById(helixId).Wait();

            return base.Deletar(id);
        }
    }
}
