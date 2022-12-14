using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels
{
    public class JsonResponse
    {
        public bool Valido { get; set; }
        public bool Erro { get => !string.IsNullOrEmpty(MessageErro); }
        public string MessageErro { get; set; }
        public string Message { get; set; }
        public string Html { get; set; }

        public JsonResponse(bool valido, string html = null, string message = null)
        {
            Valido = valido;
            Html = !string.IsNullOrEmpty(html) ? html : null;
            Message = !string.IsNullOrEmpty(message) ? message : null;
        }

        public JsonResponse(string messageErro = null)
        {
            MessageErro = !string.IsNullOrEmpty(messageErro) ? messageErro : null;
        }
    }
}
