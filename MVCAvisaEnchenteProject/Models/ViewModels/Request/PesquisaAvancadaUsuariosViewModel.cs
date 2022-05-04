using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.Request
{
    public class PesquisaAvancadaUsuariosViewModel
    {
        [DisplayName("Nome Completo")]
        [MaxLength(150)]
        public string NomeCompleto { get; set; }

        [DisplayName("Email")]
        [MaxLength(150)]
        public string Email { get; set; }

        [DisplayName("Tipo do Usuário")]
        public int? TipoUsuario { get; set; }

        public SelectList SelectListTipoUsuario => EnumHelper.GetTiposDeUsuario(TipoUsuario);

        public PesquisaAvancadaUsuariosViewModel()
        {
            NomeCompleto = string.Empty;
            Email = string.Empty;
        }
    }
}
