using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels
{
    public class EditarLocalizacaoViewModel : BaseCadastroEdicaoViewModel
    {
        [DisplayName("Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int EstadoAtendidoId { get; set; }

        [DisplayName("Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CidadeAtendidaId { get; set; }

        public EditarLocalizacaoViewModel(Usuario usuario)
        {
            CidadeAtendidaId = usuario.CidadeAtendida.Id;
            EstadoAtendidoId = usuario.EstadoAtendido.Id;
            Id = usuario.Id;
        }

        public EditarLocalizacaoViewModel()
        {

        }
    }
}
