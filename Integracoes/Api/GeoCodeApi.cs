using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes.Api
{
    public class GeoCodeApi : BaseApi
    {
        private const string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        /// <summary>
        /// permite alterar o host
        /// </summary>
        /// <param name="host"></param>
        public GeoCodeApi(string host = baseUrl) : base(host) { }
    }
}
