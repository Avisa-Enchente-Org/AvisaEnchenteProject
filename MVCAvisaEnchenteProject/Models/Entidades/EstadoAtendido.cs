using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class EstadoAtendido : BaseEntity
    {
        public string Descricao { get; set; }
        public string CodigoEstado { get; set; }
        public string UF { get; set; }

        public EstadoAtendido() { }
        public EstadoAtendido(string descricao, string codigoEstado, string uf, int? id = null) : base(id)
        {
            Descricao = descricao;
            CodigoEstado = codigoEstado;
            UF = uf;
        }
    }
}
