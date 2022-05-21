using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels
{
    public class DashboardViewModel
    {
        public RegistroSensoriamento SensoriamentoAtual { get; set; }
        public string Endereco { get; set; }

        public DashboardViewModel(RegistroSensoriamento sensoriamentoAtual, string endereco)
        {
            SensoriamentoAtual = sensoriamentoAtual;
            Endereco = endereco;
        }
    }
}
