using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.ViewComponents
{
    public class GoogleMapsViewComponent : ViewComponent
    {
        private readonly SensoriamentoAtualDAO _sensoriamentoAtualDAO;
        public GoogleMapsViewComponent()
        {
            _sensoriamentoAtualDAO = new SensoriamentoAtualDAO();
        }

        public IViewComponentResult Invoke(Usuario usuarioLogado, string enderecoUsuario)
        {
            if (usuarioLogado != null && !usuarioLogado.PrimeiroLogin)
            {
                var pontosDeSensoriamento = _sensoriamentoAtualDAO.ListarSensoriamentoAtualPorCidade(usuarioLogado.CidadeAtendida.Id);

                var mapPointCenter = new GoogleMapsModel(enderecoUsuario, pontosDeSensoriamento, usuarioLogado.CidadeAtendida.Id);
                return View(mapPointCenter);
            }

            return View(new GoogleMapsModel());
        }

    }
}
