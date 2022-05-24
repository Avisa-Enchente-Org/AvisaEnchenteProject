using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels
{
    public class PesquisaAvancadaAlertasRiscoViewModel
    {
        [DisplayName("Ponto de Sensoriamento")]
        public int? PontoDeSensoriamentoId { get; set; }

        [DisplayName("Tipo do Risco")]
        public int? TipoRisco { get; set; }

        [DisplayName("Estado")]
        public int? EstadoId { get; set; }

        [DisplayName("Cidade")]
        public int? CidadeId { get; set; }

        public PesquisaAvancadaAlertasRiscoViewModel()
        {

        }
    }
}
