using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig
{
    public static class ConexaoDB
    {
        /// <summary>
        /// Método Estático que retorna um conexao aberta com o Banco de Dados
        /// </summary>
        /// <returns>Conexão aberta com o Banco de Dados</returns>
        public static SqlConnection GetConexao()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            string strCon = configuration.GetConnectionString("DefaultConnection");
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
