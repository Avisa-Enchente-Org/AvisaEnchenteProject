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
        public CidadeAtendida() { }
        public CidadeAtendida(string descricao, string codigoCidade, int estadoAtendidoId, int? id = null) : base(id)
        {
            Descricao = descricao;
            CodigoCidade = codigoCidade;
            EstadoAtendidoId = estadoAtendidoId;
        }
    }
}
