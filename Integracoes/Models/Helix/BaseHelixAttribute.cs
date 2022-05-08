using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Models.Helix
{
    public class BaseHelixAttribute
    {
        [JsonProperty("type")]
        public virtual string Type { get; set; }

        protected BaseHelixAttribute(string type)
        {
            Type = type;
        }

        protected BaseHelixAttribute() { }
    }
}
