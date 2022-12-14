using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels
{
    public class IndexPontoDeSensoriamentoViewModel
    {
        public PesquisaAvancadaPontosDeSensoriamento PesquisaAvancadaPontosDeSensoriamento { get; set; }
        public IEnumerable<PontoDeSensoriamento> PontoDeSensoriamentos { get; set; }

        public IndexPontoDeSensoriamentoViewModel(IEnumerable<PontoDeSensoriamento> pontosDeSensoriamentos)
        {
            PesquisaAvancadaPontosDeSensoriamento = new PesquisaAvancadaPontosDeSensoriamento();
            PontoDeSensoriamentos = pontosDeSensoriamentos;
        }
    }
}
