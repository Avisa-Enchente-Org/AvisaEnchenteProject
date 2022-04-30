using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.Request
{
    public class RegistrarUsuarioViewModel
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
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(150)]
        public string Senha { get; set; }
    }
}
