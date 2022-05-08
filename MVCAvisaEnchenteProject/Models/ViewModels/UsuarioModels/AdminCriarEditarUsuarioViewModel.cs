using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.Entidades;

namespace MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels
{
    public class AdminCriarEditarUsuarioViewModel : BaseCadastroEdicaoViewModel
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

        [DisplayName("Tipo do Usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int? TipoUsuario { get; set; }

        public SelectList SelectListTipoUsuario => EnumHelper.GetTiposDeUsuario(TipoUsuario);

        public AdminCriarEditarUsuarioViewModel(Usuario usuario)
        {
            Id = usuario.Id;
            NomeCompleto = usuario.NomeCompleto;
            Email = usuario.Email;
            Senha = usuario.Senha;
            TipoUsuario = (int)usuario.TipoUsuario;
        }

        public AdminCriarEditarUsuarioViewModel()
        {
        }
    }
}
