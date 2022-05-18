using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.ParametrosNotificacao
{
    public class ParametrosNotificaoIndexViewModel
    {
        public PontoDeSensoriamento PontoSensoriamento { get; set; }
        public List<ParametroNotificacao> ParametrosNotificacao { get; set; }

        public ParametrosNotificaoIndexViewModel(PontoDeSensoriamento pds, List<ParametroNotificacao> parametrosNotificacao)
        {
            PontoSensoriamento = pds;
            PontoSensoriamento.HelixId = pds.HelixId.Replace("urn:ngsi-ld:entity:", "");
            ParametrosNotificacao = parametrosNotificacao;
        }
    }
}
