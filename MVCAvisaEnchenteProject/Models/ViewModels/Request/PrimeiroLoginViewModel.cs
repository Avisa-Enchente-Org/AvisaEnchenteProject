using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.Request
{
    public class PrimeiroLoginViewModel
    {
        [DisplayName("Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int EstadoAtendidoId { get; set; }

        [DisplayName("Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CidadeAtendidaId { get; set; }
    }
}
