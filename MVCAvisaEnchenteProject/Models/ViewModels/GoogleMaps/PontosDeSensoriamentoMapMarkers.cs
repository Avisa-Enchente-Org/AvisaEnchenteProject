using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.GoogleMaps
{
    public class PontosDeSensoriamentoMapMarkers
    {
        public int Id { get; set; }
        public Position Position { get; set; }
        public int TipoRisco { get; set; }

        public PontosDeSensoriamentoMapMarkers(int id, decimal latitude, decimal longitude, int tipoRisco)
        {
            Id = id;
            Position = new Position(latitude, longitude);
            TipoRisco = tipoRisco;
        }
    }
    public class Position
    {
        public string Lat { get; set; }
        public string Lng { get; set; }

        public Position(decimal latitude, decimal longitude)
        {
            Lat = latitude.ToString().Replace(",", ".");
            Lng = longitude.ToString().Replace(",", ".");
        }
    }
}
