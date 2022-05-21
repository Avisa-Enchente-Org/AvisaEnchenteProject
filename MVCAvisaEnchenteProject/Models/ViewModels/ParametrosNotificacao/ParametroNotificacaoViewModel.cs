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
    public class ParametroNotificacaoViewModel
    {
        public int PontoDeSensoriamentoId { get; set; }

        [DisplayName("Intensidade da Chuva")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Max(1000, ErrorMessage = "O valor máximo desse campo é de 1000")]
        public double? NivelPluviosidade { get; set; }

        [DisplayName("Vazão da Água")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Max(1000, ErrorMessage = "O valor máximo desse campo é de 1000")]
        public double? VazaoDaAgua { get; set; }

        [DisplayName("Altura da Água")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Max(1000, ErrorMessage = "O valor máximo desse campo é de 1000")]
        public double? AlturaAgua { get; set; }
        public ETipoRisco TipoRisco { get; set; }
        public bool IsCriacao { get; set; }

        public ParametroNotificacaoViewModel(ETipoRisco tipoRisco)
        {
            TipoRisco = tipoRisco;
            IsCriacao = true;
        }

        public ParametroNotificacaoViewModel(ParametroNotificacao parametroNotificacao)
        {
            PontoDeSensoriamentoId = parametroNotificacao.PontoDeSensoriamentoId;
            AlturaAgua = parametroNotificacao.AlturaAgua;
            NivelPluviosidade = parametroNotificacao.NivelPluviosidade;
            VazaoDaAgua = parametroNotificacao.VazaoDaAgua;
            TipoRisco = parametroNotificacao.TipoRisco;
            IsCriacao = false;
        }
        public ParametroNotificacaoViewModel()
        {

        }
    }
}
