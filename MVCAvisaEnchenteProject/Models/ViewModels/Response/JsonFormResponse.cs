using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.Response
{
    public class JsonFormResponse
    {
        public bool Valido { get; set; }
        public bool Erro { get => !string.IsNullOrEmpty(MessageErro); }
        public string MessageErro { get; set; }
        public string Html { get; set; }

        public JsonFormResponse(bool valido, string html = null)
        {
            Valido = valido;
            Html = !string.IsNullOrEmpty(html) ? html : null;
        }

        public JsonFormResponse(string messageErro = null)
        {
            MessageErro = !string.IsNullOrEmpty(messageErro) ? messageErro : null;
        }
    }
}
