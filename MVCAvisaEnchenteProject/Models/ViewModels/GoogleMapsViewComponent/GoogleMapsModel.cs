using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.GoogleMapsViewComponent
{
    public class GoogleMapsModel
    {
        public string EnderecoCentral { get; set; }
        public List<PontosDeSensoriamentoMapMarkers> PontosDeSensoriamentosMarkers { get; set; } = new List<PontosDeSensoriamentoMapMarkers>();

        public GoogleMapsModel(string endereco, List<PontoDeSensoriamento> pontoDeSensoriamentos)
        {
            EnderecoCentral = endereco;
            PontosDeSensoriamentosMarkers.AddRange(pontoDeSensoriamentos.Select(p => new PontosDeSensoriamentoMapMarkers(p.Id, p.Latitude, p.Longitude)));
        }

        public GoogleMapsModel()
        { 
        }
    }
}
