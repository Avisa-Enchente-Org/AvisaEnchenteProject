using System;

namespace MVCAvisaEnchenteProject.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string Erro { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel()
        {

        }
        public ErrorViewModel(string erro)
        {
            Erro = erro;
        }
    }
}
