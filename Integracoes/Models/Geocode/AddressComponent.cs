using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Integracoes.Models.Geocode
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
}