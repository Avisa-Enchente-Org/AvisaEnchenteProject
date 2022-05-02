using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class Usuario : BaseEntity
    {
        public string NomeCompleto { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public ETipoUsuario TipoUsuario { get; set; }

        public int CidadeAtendidaId { get; set; }

        public bool PrimeiroLogin { get; set; }

        public Usuario()  { }

        public Usuario(RegistrarUsuarioViewModel usuarioRequest)
        {
            NomeCompleto = usuarioRequest.NomeCompleto;
            Email = usuarioRequest.Email;
            Senha = usuarioRequest.Senha;
            TipoUsuario = ETipoUsuario.Comum;
            PrimeiroLogin = true;
        }

        public Usuario(AdminCriarEditarUsuarioViewModel usuarioRequest)
        {
            NomeCompleto = usuarioRequest.NomeCompleto;
            Email = usuarioRequest.Email;
            Senha = usuarioRequest.Senha;
            TipoUsuario = (ETipoUsuario)usuarioRequest.TipoUsuario;
            PrimeiroLogin = true;
        }
    }
}
