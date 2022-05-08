using MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels;
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
        public EstadoAtendido EstadoAtendido { get; set; }
        public Usuario Usuario { get; set; }

        public PontoDeSensoriamento(CriarEditarPontoDeSensoriamentoViewModel pontoDeSensoriamentoViewModel, int cidadeAtendidaId, int usuarioId) : base(pontoDeSensoriamentoViewModel.Id)
        {
            HelixId = $"urn:ngsi-ld:entity:{pontoDeSensoriamentoViewModel.HelixId}";
            AtivoHelix = pontoDeSensoriamentoViewModel.Ativo;
            Latitude = Convert.ToDecimal(pontoDeSensoriamentoViewModel.Latitude.Replace(".", ","));
            Longitude = Convert.ToDecimal(pontoDeSensoriamentoViewModel.Longitude.Replace(".", ","));
            CidadeAtendidaId = cidadeAtendidaId;
            UsuarioId = usuarioId;
        }
        public PontoDeSensoriamento()
        { 
        
        }
    }
}
