using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integracoes.Api
{
    public abstract class BaseApi
    {
        protected string Host { get; set; }
        protected RestClient Client { get; set; }

        public BaseApi(string host)
        {
            Host = host;
            this.Client = new RestClient(Host);
        }

        public virtual async Task<RestResponse> ExecuteRequestAsync(string endpoint, Dictionary<string, string> querieParameters, Method method, Dictionary<string, string> headers)
        {
            RestRequest request = BuildRequest(endpoint, method, headers);

            try
            {
                foreach (var param in querieParameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na construção do corpo da requisição(Query).", ex);
            }

            return await this.Client.ExecuteAsync(request);
        }

        public virtual async Task<RestResponse> ExecuteRequestAsync(string endpoint, object body, Method method, Dictionary<string, string> headers)
        {
            RestRequest request = BuildRequest(endpoint, method, headers);

            try
            {
                request.AddJsonBody(body);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na construção do corpo da requisição(Json).", ex);
            }

            return await this.Client.ExecuteAsync(request);
        }

        private static RestRequest BuildRequest(string endpoint, Method method, Dictionary<string, string> requestHeaders)
        {
            try
            {
                var request = new RestRequest(endpoint, method);

                if (requestHeaders != null && requestHeaders.Count > 0)
                {
                    foreach (var header in requestHeaders)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao construir o header da requisição.", ex);
            }
        }
    }
}
