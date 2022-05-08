using System;
using System.Collections.Generic;
using System.Text;

namespace Integracoes
{
    public abstract class IntegracaoBase
    {

        /// <summary>
        /// Gera o Header basico para requisições
        /// </summary>
        protected virtual Dictionary<string, string> GerarHeaderBasico()
        {
            var listRequestHeader = new Dictionary<string, string>
            {
                { "accept", "application/json" }
            };

            return listRequestHeader;
        }
    }
}
