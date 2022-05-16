using Integracoes;
using Integracoes.Api;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Models.Enum;

namespace MVCAvisaEnchenteProject.Controllers
{

    [Authorize]
    public class LocalizacaoController : ControllerBase
    {
        private readonly IntegracaoIBGE _integracaoIBGE;
        private readonly EstadoAtendidoDAO _estadoAtendidoDAO;
        private readonly CidadeAtendidaDAO _cidadeAtendidaDAO;
        public LocalizacaoController()
        {
            _integracaoIBGE = new IntegracaoIBGE(new IBGEApi());
            _estadoAtendidoDAO = new EstadoAtendidoDAO();
            _cidadeAtendidaDAO = new CidadeAtendidaDAO();
        }

        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> ObtemSelectListEstados()
        {
            var estados = await _integracaoIBGE.ListarEstados();
            List<SelectListItem> selectEstados = new List<SelectListItem>();
            estados.ToList().ForEach(x =>
            {
                selectEstados.Add(new SelectListItem { Text = x.Nome, Value = x.Id.ToString() });
            });

            return Ok(selectEstados);
        }

        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> ObtemSelectListCidadesPorUF(string estado)
        {
            List<SelectListItem> selectCidades = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(estado))
            {
                var cidades = await _integracaoIBGE.ListarCidades(estado);
                cidades.ToList().ForEach(x =>
                {
                    selectCidades.Add(new SelectListItem { Text = x.Nome, Value = x.Id.ToString() });
                });

            }
            return Ok(selectCidades);
        }

        [HttpGet]
        public IActionResult ObtemSelectListEstadosAtendidos()
        {
            var estadoDAO = new EstadoAtendidoDAO();
            var estados = estadoDAO.Listar();
            List<SelectListItem> selectEstados = new List<SelectListItem>();
            estados.ToList().ForEach(x =>
            {
                selectEstados.Add(new SelectListItem { Text = x.Descricao, Value = x.Id.ToString() });
            });

            return Ok(selectEstados);
        }

        [HttpGet]
        public IActionResult ObtemSelectListCidadesAtendidasPorEstadoId(int estado)
        {
            List<SelectListItem> selectCidades = new List<SelectListItem>();

            var cidades = _cidadeAtendidaDAO.ListarCidadesAtendidasPorEstadoId(estado);
            cidades.ToList().ForEach(x =>
            {
                selectCidades.Add(new SelectListItem { Text = x.Descricao, Value = x.Id.ToString() });
            });

            return Ok(selectCidades);
        }


    }
}
