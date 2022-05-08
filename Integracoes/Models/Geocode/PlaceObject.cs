using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Integracoes.Models.Geocode
{
    public class PlaceObject
    {
        public List<Result> results { get; set; }

        public string status { get; set; }
    }
}
