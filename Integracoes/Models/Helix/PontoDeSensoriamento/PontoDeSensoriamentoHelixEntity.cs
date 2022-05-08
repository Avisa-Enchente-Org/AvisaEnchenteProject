using Integracoes.Models.Helix.PontoDeSensoriamento.Atributos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Models.Helix.PontoDeSensoriamento
{
    public class PontoDeSensoriamentoHelixEntity : BaseHelixEntity<PontoDeSensoriamentoHelixEntity>
    {
        public override string Type { get => "PontoSensoriamento"; }

        [JsonProperty("rainintensity")]
        public RainIntensity RainIntensity { get; set; }

        [JsonProperty("waterheight")]
        public WaterHeight WaterHeight { get; set; }

        [JsonProperty("flowratewater")]
        public FlowrateWater FlowrateWater { get; set; }

        [JsonProperty("lastupdate")]
        public LastUpdate LastUpdate { get; set; }

        public PontoDeSensoriamentoHelixEntity(string id) : base(id) 
        {
            RainIntensity = new RainIntensity();
            WaterHeight = new WaterHeight();
            FlowrateWater = new FlowrateWater();
            LastUpdate = new LastUpdate();
        }
        public PontoDeSensoriamentoHelixEntity() : base() { }

    }
}
