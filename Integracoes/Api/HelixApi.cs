using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Api
{
    public class HelixApi : BaseApi
    {
        private const string localUrl = "http://10.5.10.34:1026/";

        /// <summary>
        /// permite alterar o host
        /// </summary>
        /// <param name="host"></param>
        public HelixApi(string host = localUrl) : base(host) { }
    }
}
