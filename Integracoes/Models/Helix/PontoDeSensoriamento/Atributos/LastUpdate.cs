using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Models.Helix.PontoDeSensoriamento.Atributos
{
    public class LastUpdate : BaseHelixAttribute
    {
        public override string Type { get => "string"; }

        [JsonProperty("value")]
        public string Value { get; set; }
        public LastUpdate()
        {
            Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");       
        }
    }
}
