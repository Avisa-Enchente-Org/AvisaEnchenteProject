using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Api
{
    public class IBGEApi : BaseApi
    {
        private const string baseUrl = "https://servicodados.ibge.gov.br/api/v1";

        /// <summary>
        /// permite alterar o host
        /// </summary>
        /// <param name="host"></param>
        public IBGEApi(string host = baseUrl) : base(host) { }
    }
}
