using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels.ParametrosNotificacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class ParametroNotificacao : BaseEntity
    {
        public int PontoDeSensoriamentoId { get; set; }
        public double NivelPluviosidade { get; set; }
        public double VazaoDaAgua { get; set; }
        public double AlturaAgua { get; set; }
        public ETipoRisco TipoRisco { get; set; }
        public PontoDeSensoriamento PontoDeSensoriamento { get; set; }

        public ParametroNotificacao(ParametroNotificacaoViewModel parametroNotificacaoViewModel)
        {
            Id = parametroNotificacaoViewModel.Id;
            PontoDeSensoriamentoId = parametroNotificacaoViewModel.PontoDeSensoriamentoId;
            NivelPluviosidade = parametroNotificacaoViewModel.NivelPluviosidade.Value;
            VazaoDaAgua = parametroNotificacaoViewModel.VazaoDaAgua.Value;
            AlturaAgua = parametroNotificacaoViewModel.AlturaAgua.Value;
            TipoRisco = parametroNotificacaoViewModel.TipoRisco;
        }

        public ParametroNotificacao()
        {

        }

    }
}
