using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO
{
    public class CidadeAtendidaDAO : BaseDAO<CidadeAtendida>
    { 
        protected override void SetTabela()
        {
            Tabela = "cidades_atendidas";
        }

        public List<CidadeAtendida> ListarCidadesAtendidasPorEstadoId(int estadoId)
        {
            SqlParameter[] estadoIdParametro = {
                    new SqlParameter("estado_atendido_id", estadoId),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_listar_cidades_atendidas_por_estado", estadoIdParametro);

            var lista = new List<CidadeAtendida>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        public CidadeAtendida ConsultaCidadePorCodigo(string codigoCidade)
        {
            SqlParameter[] codigoParametro = {
                    new SqlParameter("codigo", codigoCidade),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_cidade_atendida_por_codigo", codigoParametro);

            if (tabela.Rows.Count != 0)
                return MontaEntidadePadrao(tabela.Rows[0]);
            else
                return null;
        }

        protected override SqlParameter[] CriaParametros(CidadeAtendida cidadeAtendida)
        {
            var estadoParametros = new List<SqlParameter>
                {
                    new SqlParameter("descricao", cidadeAtendida.Descricao),
                    new SqlParameter("codigo_cidade", cidadeAtendida.CodigoCidade),
                    new SqlParameter("estado_atendido_id", cidadeAtendida.EstadoAtendidoId),
                    new SqlParameter("latitude_ref", cidadeAtendida.LatitudeRef),
                    new SqlParameter("longitude_ref", cidadeAtendida.LongitudeRef)
                };
            if (cidadeAtendida.Id > 0)
                estadoParametros.Add(new SqlParameter("id", cidadeAtendida.Id));

            return estadoParametros.ToArray();
        }

        protected override CidadeAtendida MontaEntidadePadrao(DataRow registro)
        {
            var cidadeAtendida = new CidadeAtendida
            {
                Id = Convert.ToInt32(registro["id"]),
                Descricao = registro["descricao"].ToString(),
                CodigoCidade = registro["codigo_cidade"].ToString(),
                EstadoAtendidoId = Convert.ToInt32(registro["estado_atendido_id"]),
                LatitudeRef = Convert.ToDecimal(registro["latitude_ref"]),
                LongitudeRef = Convert.ToDecimal(registro["longitude_ref"]),
            };

            return cidadeAtendida;
        }
    
    }
}
