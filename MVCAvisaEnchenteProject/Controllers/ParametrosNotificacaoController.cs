using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
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

        public override IActionResult Index(int id = 0)
        {
            var pds = _pontoSensoriamentoDAO.ConsultarPorId(id);
            if (pds == null)
                return RedirectToAction("Index", "SensoriamentoAtual");

            var parametrosNotificacao = DAOPrincipal.ListarParametrosNotificacaoPorPDS(pds.Id);

            return View(id);
        }

    }
}
