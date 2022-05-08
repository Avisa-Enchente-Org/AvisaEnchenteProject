using Integracoes.Api;
using Integracoes.Models.Geocode;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integracoes
{
    public class IntegracaoGeocode : IntegracaoBase
    {
        private readonly GeoCodeApi _geocodeApi;
        private readonly string apiKey = "AIzaSyCmKEgJE1bL32pc7W8MudLTLQ2I1seN0VQ";
        public IntegracaoGeocode(GeoCodeApi api)
        {
            _geocodeApi = api;
        }

        /// <summary>
        /// Método de busca de localização a partir de uma latitude e longitude
        /// </summary>
        /// <returns>Retorna o objeto de localização</returns>
        public async Task<PlaceObject> ObtemLocalizacaoPorLatLng(string latitude, string longitude)
        {
            var header = GerarHeaderBasico();

            var queriesParameters = new Dictionary<string, string>()
            {
                { "latlng", $"{latitude.Replace(",", ".")},{longitude.Replace(",", ".")}" },
                { "key", apiKey }
            };

            var response = await _geocodeApi.ExecuteRequestAsync(string.Empty, queriesParameters, Method.Get, header);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<PlaceObject>(response.Content);

            return null;
        }
    }
}
