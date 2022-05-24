using Integracoes.Models.Helix.PontoDeSensoriamento.Atributos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Models.Helix.PontoDeSensoriamento
{
    public class PontoDeSensoriamentoHelixEntity : BaseHelixEntity<PontoDeSensoriamentoHelixEntity>
    {
        public override string Type { get => "sensor"; }

        [JsonProperty("rainintensity")]
        public RainIntensity RainIntensity { get; set; }

        [JsonProperty("riverheight")]
        public WaterHeight RiverHeight { get; set; }

        [JsonProperty("riverflowrate")]
        public FlowrateWater RiverFlowrate { get; set; }

        [JsonProperty("lastupdate")]
        public LastUpdate LastUpdate { get; set; }
        
        public PontoDeSensoriamentoHelixEntity(string id) : base(id) 
        {
            RainIntensity = new RainIntensity();
            RiverHeight = new WaterHeight();
            RiverFlowrate = new FlowrateWater();
            LastUpdate = new LastUpdate();
        }
        public PontoDeSensoriamentoHelixEntity() : base() { }

    }
}
