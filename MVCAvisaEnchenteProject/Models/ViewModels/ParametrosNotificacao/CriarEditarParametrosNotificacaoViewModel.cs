using DataAnnotationsExtensions;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.ParametrosNotificacao
{
    public class CriarEditarParametrosNotificacaoViewModel
    {
        public List<ParametroNotificacaoViewModel> ParametrosNotificacaoModel { get; set; }
        public bool isEdicao => ParametrosNotificacaoModel.Any(p => p.EdicaoModel());
        public int PontoDeSensoriamentoId { get; set; }

        public CriarEditarParametrosNotificacaoViewModel(int pdsId)
        {
            ParametrosNotificacaoModel = new List<ParametroNotificacaoViewModel>
            {
                new ParametroNotificacaoViewModel(ETipoRisco.BaixoRisco),
                new ParametroNotificacaoViewModel(ETipoRisco.MedioRisco),
                new ParametroNotificacaoViewModel(ETipoRisco.AltoRisco),
            };
            PontoDeSensoriamentoId = pdsId;
        }
        public CriarEditarParametrosNotificacaoViewModel(List<ParametroNotificacao> parametrosNotificacao, int pdsId)
        {
            ParametrosNotificacaoModel = new List<ParametroNotificacaoViewModel>();
            parametrosNotificacao.ForEach(p => ParametrosNotificacaoModel.Add(new ParametroNotificacaoViewModel(p)));
            PontoDeSensoriamentoId = pdsId;
        }
        public CriarEditarParametrosNotificacaoViewModel()
        {

        }
    }

}
