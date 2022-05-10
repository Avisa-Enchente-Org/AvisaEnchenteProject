using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMapsViewComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.ViewComponents
{
    public class GoogleMapsViewComponent : ViewComponent
    {
        private readonly UsuarioDAO _usuarioDAO;
        private readonly CidadeAtendidaDAO _cidadeDAO;
        private readonly EstadoAtendidoDAO _estadoDAO;
        private readonly PontoDeSensoriamentoDAO _pdsDAO;
        public GoogleMapsViewComponent()
        {
            _usuarioDAO = new UsuarioDAO();
            _cidadeDAO = new CidadeAtendidaDAO();
            _estadoDAO = new EstadoAtendidoDAO();
            _pdsDAO = new PontoDeSensoriamentoDAO();
        }
        public IViewComponentResult Invoke()
        {
            var userStringId = HttpContext.User?.FindFirst("UsuarioId")?.Value;
            if (int.TryParse(userStringId, out int usuarioId))
            {         
                var usuarioEnderecoLogado = _usuarioDAO.ConsultarEnderecoUsuario(usuarioId);
                if(usuarioEnderecoLogado != null)
                {
                    var enderecoUsuario = $"{usuarioEnderecoLogado.CidadeAtendida.Descricao}, {usuarioEnderecoLogado.EstadoAtendido.Descricao}";

                    var pontosDeSensoriamento = _pdsDAO.ListarPorCidade(usuarioEnderecoLogado.CidadeAtendida.Id);

                    var mapPointCenter = new GoogleMapsModel(enderecoUsuario, pontosDeSensoriamento);
                    return View(mapPointCenter);
                }

                return View(new GoogleMapsModel());
            }

            return View(new GoogleMapsModel());
        }
    }
}
