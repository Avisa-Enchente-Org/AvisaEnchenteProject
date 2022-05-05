using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO
{
    public class PontoDeSensoriamentoDAO : BaseDAO<PontoDeSensoriamento>
    {
        protected override string NomeSpListagem { get; set; } = "sp_listar_pontos_sensoriamento";
        protected override void SetTabela()
        {
            Tabela = "pontos_sensoriamento";
        }



        protected override SqlParameter[] CriaParametros(PontoDeSensoriamento pontoDeSensoriamento)
        {
            SqlParameter[] pontoDeSensoriamentoParametros = {
                new SqlParameter("id", pontoDeSensoriamento.Id),
                new SqlParameter("helix_id", pontoDeSensoriamento.HelixId),
                new SqlParameter("ativo_helix", pontoDeSensoriamento.AtivoHelix),
                new SqlParameter("cidade_atendida_id", pontoDeSensoriamento.CidadeAtendidaId),
                new SqlParameter("latitude", pontoDeSensoriamento.Latitude),
                new SqlParameter("longitude", pontoDeSensoriamento.Longitude),
                new SqlParameter("usuario_id", pontoDeSensoriamento.UsuarioId),
            };

            return pontoDeSensoriamentoParametros;
        }

        protected override PontoDeSensoriamento MontaEntidadePadrao(DataRow registro)
        {
            var pontoDeSensoriamento = new PontoDeSensoriamento
            {
                Id = Convert.ToInt32(registro["id"]),
                HelixId = registro["helix_id"].ToString(),
                AtivoHelix = Convert.ToBoolean(registro["ativo_helix"]),
                Latitude = Convert.ToDecimal(registro["latitude"]),
                Longitude = Convert.ToDecimal(registro["longitude"]),
                Usuario = new Usuario
                {
                    Id = Convert.ToInt32(registro["usuario_id"]),
                    NomeCompleto = registro["nome_completo_usuario"].ToString(),
                },
                CidadeAtendida = new CidadeAtendida
                {
                    Id = Convert.ToInt32(registro["cidade_atendida_id"]),
                    Descricao = registro["cidade_descricao"].ToString(),
                    CodigoCidade = registro["helix_id"].ToString(),
                },
                EstadoeAtendido = new EstadoAtendido
                {
                    Id = Convert.ToInt32(registro["estado_atendido_id"]),
                    Descricao = registro["estado_descricao"].ToString(),
                    UF = registro["estado_uf"].ToString(),
                    CodigoEstado = registro["codigo_estado"].ToString()
                }
            };

            return pontoDeSensoriamento;
        }
    }
}
