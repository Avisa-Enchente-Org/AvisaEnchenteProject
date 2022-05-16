using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class NotificacaoHistorico : BaseEntity
    {
        public int PontoDeSensoriamentoId { get; set; }
        public double NivelPluviosidade { get; set; }
        public double VazaoDaAgua { get; set; }
        public double AlturaAgua { get; set; }
        public ETipoRisco TipoRisco { get; set; }
        public DateTime DataNotificacao { get; set; }
        public PontoDeSensoriamento PontoDeSensoriamento { get; set; }
    }
}
