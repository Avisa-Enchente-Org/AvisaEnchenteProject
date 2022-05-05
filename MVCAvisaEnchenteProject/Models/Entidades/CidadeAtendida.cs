using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class CidadeAtendida : BaseEntity
    {
        public string Descricao { get; set; }
        public string CodigoCidade { get; set; }
        public int EstadoAtendidoId { get; set; }
        public decimal LatitudeRef { get; set; }
        public decimal LongitudeRef { get; set; }

        public CidadeAtendida() { }
        public CidadeAtendida(string descricao, string codigoCidade, int estadoAtendidoId, decimal latitudeRef, decimal longitudeRef, int? id = null) : base(id)
        {
            Descricao = descricao;
            CodigoCidade = codigoCidade;
            EstadoAtendidoId = estadoAtendidoId;
            LatitudeRef = latitudeRef;
            LongitudeRef = longitudeRef;
        }
    }
}
