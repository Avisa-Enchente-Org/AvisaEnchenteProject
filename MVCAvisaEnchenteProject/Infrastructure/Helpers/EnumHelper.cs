using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {

            if (!typeof(T).IsEnum)
                return string.Empty;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description ?? string.Empty;
        }


        public static SelectList GetTiposDeUsuario(int? tipoDeUsuario)
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = ETipoUsuario.Comum.GetDescription(), Value = ((int) ETipoUsuario.Comum).ToString()},
                new SelectListItem { Text = ETipoUsuario.Admin.GetDescription(), Value = ((int) ETipoUsuario.Admin).ToString()}
            }, "Value", "Text", tipoDeUsuario);
        }

        public static SelectList GetTiposDeRisco(int? tipoDeRisco)
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = ETipoRisco.BaixoRisco.GetDescription(), Value = ((int) ETipoRisco.BaixoRisco).ToString()},
                new SelectListItem { Text = ETipoRisco.MedioRisco.GetDescription(), Value = ((int) ETipoRisco.MedioRisco).ToString()},
                new SelectListItem { Text = ETipoRisco.AltoRisco.GetDescription(), Value = ((int) ETipoRisco.AltoRisco).ToString()},
            }, "Value", "Text", tipoDeRisco);
        }
    }
}
