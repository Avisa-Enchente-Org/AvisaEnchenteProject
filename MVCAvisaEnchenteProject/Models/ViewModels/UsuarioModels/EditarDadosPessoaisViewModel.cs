using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels
{
    public class EditarDadosPessoaisViewModel : BaseCadastroEdicaoViewModel
    {

        [DisplayName("Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(150)]
        public string NomeCompleto { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Digite o Email Corretamente")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(150)]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [MaxLength(150)]
        public string Senha { get; set; }

        public EditarDadosPessoaisViewModel(Usuario usuario)
        {
            Id = usuario.Id;
            NomeCompleto = usuario.NomeCompleto;
            Email = usuario.Email;
            Senha = usuario.Senha;
        }

        public EditarDadosPessoaisViewModel()
        {
        }
    }
}
