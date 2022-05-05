using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.Entidades
{
    public class PontoDeSensoriamento : BaseEntity
    {
        public string HelixId { get; set; }
        public bool AtivoHelix { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int CidadeAtendidaId { get; set; }
        public int UsuarioId { get; set; }
        public CidadeAtendida CidadeAtendida { get; set; }
        public EstadoAtendido EstadoeAtendido { get; set; }
        public Usuario Usuario { get; set; }
    }
}
