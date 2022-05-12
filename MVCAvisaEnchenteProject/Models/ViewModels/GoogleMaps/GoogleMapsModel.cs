using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps
{
    public class GoogleMapsModel
    {
        public string EnderecoCentral { get; set; }
        public List<PontosDeSensoriamentoMapMarkers> PontosDeSensoriamentosMarkers { get; set; } = new List<PontosDeSensoriamentoMapMarkers>();

        public GoogleMapsModel(string endereco, List<SensoriamentoAtual> pontoDeSensoriamentos)
        {
            EnderecoCentral = endereco;
            PontosDeSensoriamentosMarkers.AddRange(pontoDeSensoriamentos.Select(p => new PontosDeSensoriamentoMapMarkers(p.PontoDeSensoriamento.Id, p.PontoDeSensoriamento.Latitude, p.PontoDeSensoriamento.Longitude, (int)p.TipoRisco)));
        }

        public GoogleMapsModel()
        { 
        }
    }
}
