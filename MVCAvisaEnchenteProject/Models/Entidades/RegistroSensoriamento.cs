using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class RegistroSensoriamento : BaseEntity
    {
        public int PontoDeSensoriamentoId { get; set; }
        public double NivelPluviosidade { get; set; }
        public double VazaoDaAgua { get; set; }
        public double AlturaAgua { get; set; }
        public DateTime DataRegistro { get; set; }
        public ETipoRisco TipoRisco { get; set; }
        public string TipoRiscoTexto => TipoRisco.GetDescription();
        public PontoDeSensoriamento PontoDeSensoriamento { get; set; }
        public RegistroSensoriamento()
        {
                    
        }
        public RegistroSensoriamento(int pontoDeSensoriamentoId)
        {
            PontoDeSensoriamentoId = pontoDeSensoriamentoId;
        }
    }
}
