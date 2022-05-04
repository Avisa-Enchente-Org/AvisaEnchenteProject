using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Enum
{
    public enum ETipoUsuario
    {
        [Description("Comum")]
        Comum = 1,

        [Description("Administrador")]
        Admin = 2
    }

    public static class TipoDeUsuarioHelper
    {
        public static SelectList GetTiposDeUsuario(int? tipoDeUsuario)
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = ETipoUsuario.Comum.GetDescription(), Value = ((int) ETipoUsuario.Comum).ToString()},
                new SelectListItem { Text = ETipoUsuario.Admin.GetDescription(), Value = ((int) ETipoUsuario.Admin).ToString()}
            }, "Value", "Text", tipoDeUsuario);
        }
    }
}
