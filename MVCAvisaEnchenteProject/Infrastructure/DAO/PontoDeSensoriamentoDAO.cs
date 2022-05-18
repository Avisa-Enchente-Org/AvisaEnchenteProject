using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels;
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
        protected override void SetTabela()
        {
            Tabela = "pontos_sensoriamento";
        }

        public List<PontoDeSensoriamento> PesquisaAvancadaPontosSensoriamento(PesquisaAvancadaPontosDeSensoriamento pesquisaAvancadaPontosDeSensoriamento)
        {
            var ativo = pesquisaAvancadaPontosDeSensoriamento.Ativo;
            var p = new SqlParameter[]
            {
                new SqlParameter("helixId", pesquisaAvancadaPontosDeSensoriamento.HelixId ?? ""),
                new SqlParameter("ativo", ativo.HasValue ? (ativo.Value ? "1" : "0"): ""),
                new SqlParameter("usuarioId", pesquisaAvancadaPontosDeSensoriamento.UsuarioId.HasValue && pesquisaAvancadaPontosDeSensoriamento.UsuarioId.Value != default ? pesquisaAvancadaPontosDeSensoriamento.UsuarioId.Value.ToString() : ""),
                new SqlParameter("cidadeId", pesquisaAvancadaPontosDeSensoriamento.CidadeId.HasValue && pesquisaAvancadaPontosDeSensoriamento.CidadeId.Value != default ? pesquisaAvancadaPontosDeSensoriamento.CidadeId.Value.ToString() : ""),
                new SqlParameter("estadoId", pesquisaAvancadaPontosDeSensoriamento.EstadoId.HasValue && pesquisaAvancadaPontosDeSensoriamento.EstadoId.Value != default ? pesquisaAvancadaPontosDeSensoriamento.EstadoId.Value.ToString() : ""),
            };

            var lista = new List<PontoDeSensoriamento>();
            DataTable tabela = HelperDAO.ExecutaProcSelect("sp_pesquisa_avancada_pontos_sensoriamento", p);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        public bool PontoDeSensoriamentoJaExiste(string helixId)
        {
            SqlParameter[] helixIdParametro = {
                new SqlParameter("helixId", helixId),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_ponto_de_sensoriamento_por_helixid", helixIdParametro);

            if (tabela.Rows.Count == 0)
                return false;
            else
                return true;
        }

        public override PontoDeSensoriamento ConsultarPorId(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consultar_pontos_sensoriamento", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaEntidadePadrao(tabela.Rows[0]);
        }

        protected override SqlParameter[] CriaParametros(PontoDeSensoriamento pontoDeSensoriamento)
        {
            var pontoDeSensoriamentoParametros = new List<SqlParameter>
            {
                new SqlParameter("helix_id", pontoDeSensoriamento.HelixId),
                new SqlParameter("ativo_helix", pontoDeSensoriamento.AtivoHelix),
                new SqlParameter("cidade_atendida_id", pontoDeSensoriamento.CidadeAtendidaId),
                new SqlParameter("latitude", pontoDeSensoriamento.Latitude),
                new SqlParameter("longitude", pontoDeSensoriamento.Longitude),
                new SqlParameter("usuario_id", pontoDeSensoriamento.UsuarioId),
            };
            if (pontoDeSensoriamento.Id > 0)
                pontoDeSensoriamentoParametros.Add(new SqlParameter("id", pontoDeSensoriamento.Id));

            return pontoDeSensoriamentoParametros.ToArray();
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
                UsuarioId = Convert.ToInt32(registro["usuario_id"]),
                CidadeAtendidaId = Convert.ToInt32(registro["cidade_atendida_id"]),
                Usuario = new Usuario
                {
                    Id = Convert.ToInt32(registro["usuario_id"]),
                    NomeCompleto = registro["nome_completo_usuario"].ToString(),
                },
                CidadeAtendida = new CidadeAtendida
                {
                    Id = Convert.ToInt32(registro["cidade_atendida_id"]),
                    Descricao = registro["cidade_descricao"].ToString(),
                    CodigoCidade = registro["codigo_cidade"].ToString(),
                },
                EstadoAtendido = new EstadoAtendido
                {
                    Id = Convert.ToInt32(registro["estado_atendido_id"]),
                    Descricao = registro["estado_descricao"].ToString(),
                    UF = registro["estado_uf"].ToString(),
                    CodigoEstado = registro["codigo_estado"].ToString()
                }
            };

            return pontoDeSensoriamento;
        }

        internal object ConsultarPorHelixId(int value)
        {
            throw new NotImplementedException();
        }
    }
}
