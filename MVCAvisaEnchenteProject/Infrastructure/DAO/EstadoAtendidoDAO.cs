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
    public class EstadoAtendidoDAO : BaseDAO<EstadoAtendido>
    {
        protected override void SetTabela()
        {
            Tabela = "estados_atendidos";
        }

        public EstadoAtendido ConsultaEstadoPorCodigo(string codigoEstado)
        {
            SqlParameter[] codigoParametro = {
                new SqlParameter("codigo", codigoEstado),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_estado_atendido_por_codigo", codigoParametro);

            if (tabela.Rows.Count != 0)
                return MontaEntidadePadrao(tabela.Rows[0]);
            else
                return null;
        }

        protected override SqlParameter[] CriaParametros(EstadoAtendido estadoAtendido)
        {
            var estadoParametros = new List<SqlParameter>
            {
                new SqlParameter("descricao", estadoAtendido.Descricao),
                new SqlParameter("uf", estadoAtendido.UF),
                new SqlParameter("codigo_estado", estadoAtendido.CodigoEstado)
            };
            if (estadoAtendido.Id > 0)
                estadoParametros.Add(new SqlParameter("id", estadoAtendido.Id));

            return estadoParametros.ToArray();
        }

        protected override EstadoAtendido MontaEntidadePadrao(DataRow registro)
        {
            var estadoAtendido = new EstadoAtendido
            {
                Id = Convert.ToInt32(registro["id"]),
                Descricao = registro["descricao"].ToString(),
                UF = registro["uf"].ToString(),
                CodigoEstado = registro["codigo_estado"].ToString()
            };         

            return estadoAtendido;
        }

    }
}
