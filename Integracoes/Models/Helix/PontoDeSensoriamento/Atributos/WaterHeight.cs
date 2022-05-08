using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Models.Helix.PontoDeSensoriamento.Atributos
{
    public class WaterHeight : BaseHelixAttribute
    {
        public override string Type => "float";

        [JsonProperty("value")]
        public double Value { get; set; }
        public WaterHeight()
        {
                
        }
    }
}
