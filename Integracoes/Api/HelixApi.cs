using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Api
{
    public class HelixApi : BaseApi
    {
        private const string localUrl = "http://localhost:1026/";

        /// <summary>
        /// permite alterar o host
        /// </summary>
        /// <param name="host"></param>
        public HelixApi(string host = localUrl) : base(host) { }
    }
}
