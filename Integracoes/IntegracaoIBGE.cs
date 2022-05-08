using Integracoes.Api;
using Integracoes.Models.IBGE;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integracoes
{
    public class IntegracaoIBGE : IntegracaoBase
    {
        private readonly IBGEApi _ibgeApi;
        public IntegracaoIBGE(IBGEApi api)
        {
            _ibgeApi = api;
        }

        /// <summary>
        /// Método reflete o endpoint GET /localidades/estados para buscar todos os estados do Brasil
        /// </summary>
        /// <returns>Retorna o um array de estados</returns>
        public async Task<Estado[]> ListarEstados()
        {
            var endpoint = $"/localidades/estados";
            var header = GerarHeaderBasico();

            var response = await _ibgeApi.ExecuteRequestAsync(endpoint, new Dictionary<string, string>(), Method.Get, header);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Estado[]>(response.Content);

            return Array.Empty<Estado>();
        }

        /// Método reflete o endpoint GET /localidades/estados para buscar todos os estados do Brasil
        /// </summary>
        /// <returns>Retorna o um array de estados</returns>
        public async Task<Estado> ObterEstadoPorCodigo(string codigoEstado)
        {
            var endpoint = $"/localidades/estados/{codigoEstado}";
            var header = GerarHeaderBasico();

            var response = await _ibgeApi.ExecuteRequestAsync(endpoint, new Dictionary<string, string>(), Method.Get, header);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Estado>(response.Content);

            return new Estado();
        }

        /// <summary>
        /// Método reflete o endpoint GET /localidades/estados para buscar todos os estados do Brasil
        /// </summary>
        /// <returns>Retorna o um array de estados</returns>
        public async Task<Municipio[]> ListarCidades(string uf)
        {
            var endpoint = $"/localidades/estados/{uf}/municipios";
            var header = GerarHeaderBasico();

            var response = await _ibgeApi.ExecuteRequestAsync(endpoint, new Dictionary<string, string>(), Method.Get, header);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Municipio[]>(response.Content);

            return Array.Empty<Municipio>();
        }

        /// Método reflete o endpoint GET /localidades/estados para buscar todos os estados do Brasil
        /// </summary>
        /// <returns>Retorna o um array de estados</returns>
        public async Task<Municipio> ObterCidadePorCodigo(string codigoCidade)
        {
            var endpoint = $"/localidades/municipios/{codigoCidade}";
            var header = GerarHeaderBasico();

            var response = await _ibgeApi.ExecuteRequestAsync(endpoint, new Dictionary<string, string>(), Method.Get, header);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Municipio>(response.Content);

            return new Municipio();
        }
    }
}
