using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.GoogleMapsViewComponent
{
    public class PontosDeSensoriamentoMapMarkers
    {
        public int Id { get; set; }
        public Location Location { get; set; }

        public PontosDeSensoriamentoMapMarkers(int id, decimal latitude, decimal longitude)
        {
            Id = id;
            Location = new Location(latitude, longitude);
        }
    }
    public class Location
    {
        public string Lat { get; set; }
        public string Lng { get; set; }

        public Location(decimal latitude, decimal longitude)
        {
            Lat = latitude.ToString().Replace(",", ".");
            Lng = longitude.ToString().Replace(",", ".");
        }
    }
}
