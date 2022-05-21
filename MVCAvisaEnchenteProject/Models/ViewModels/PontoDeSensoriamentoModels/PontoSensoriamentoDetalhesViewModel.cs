using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels
{
    public class PontoSensoriamentoDetalhesViewModel
    {
        public PontoDeSensoriamento PontoSensoriamento { get; set; }
        public List<ParametroNotificacao> ParametrosNotificacao { get; set; }

        public PontoSensoriamentoDetalhesViewModel(PontoDeSensoriamento pds, List<ParametroNotificacao> parametrosNotificacao)
        {
            PontoSensoriamento = pds;
            PontoSensoriamento.HelixId = pds.HelixId.Replace("urn:ngsi-ld:sensor:", "");
            ParametrosNotificacao = parametrosNotificacao;
        }

        public PontoSensoriamentoDetalhesViewModel()
        {

        }
    }
}
