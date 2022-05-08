using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels
{
    public class LoginUsuarioRequest
    {
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Digite o Email Corretamente")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres ")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres ")]
        public string Senha { get; set; }
    }
}
