using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Integracoes.Models.Geocode
{
    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public List<string> types { get; set; }
    }
}