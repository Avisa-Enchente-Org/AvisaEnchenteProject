using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO
{
    public class ParametroNotificacaoDAO : BaseDAO<ParametroNotificacao>
    {
        protected override void SetTabela()
        {
            Tabela = "notificacoes_parametros";
        }

        public List<ParametroNotificacao> ListarParametrosNotificacaoPorPDS(int pdsId)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ponto_sensoriamento_id", pdsId)
            };
            var tabela = HelperDAO.ExecutaProcSelect("sp_listar_notificacoes_parametros_por_pds", p);

            var lista = new List<ParametroNotificacao>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        protected override SqlParameter[] CriaParametros(ParametroNotificacao parametroNotificacao)
        {
            var pontoDeSensoriamentoParametros = new List<SqlParameter>
            {
                new SqlParameter("ponto_sensoriamento_id", parametroNotificacao.PontoDeSensoriamentoId),
                new SqlParameter("tipo_risco", parametroNotificacao.TipoRisco),
                new SqlParameter("vazao_da_agua", parametroNotificacao.VazaoDaAgua),
                new SqlParameter("nivel_pluviosidade", parametroNotificacao.NivelPluviosidade),
                new SqlParameter("altura_agua", parametroNotificacao.AlturaAgua)
            };
            if (parametroNotificacao.Id > 0)
                pontoDeSensoriamentoParametros.Add(new SqlParameter("id", parametroNotificacao.Id));

            return pontoDeSensoriamentoParametros.ToArray();
        }

        protected override ParametroNotificacao MontaEntidadePadrao(DataRow registro)
        {
            var parametroNotificacao = new ParametroNotificacao
            {
                Id = Convert.ToInt32(registro["id"]),
                TipoRisco = (ETipoRisco)Convert.ToInt32(registro["tipo_risco"]),
                NivelPluviosidade = Convert.ToDouble(registro["nivel_pluviosidade"]),
                AlturaAgua = Convert.ToDouble(registro["altura_agua"]),
                VazaoDaAgua = Convert.ToDouble(registro["vazao_agua"]),
                PontoDeSensoriamentoId = Convert.ToInt32(registro["ponto_sensoriamento_id"]) 
            };

            return parametroNotificacao;
        }

        public override List<ParametroNotificacao> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
