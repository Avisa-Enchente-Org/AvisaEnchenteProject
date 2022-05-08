using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO
{
    public class SensoriamentoAtualDAO : BaseDAO<SensoriamentoAtual>
    {
        protected override void SetTabela()
        {
            Tabela = "sensoriamento_atual";
        }

        public SensoriamentoAtual ConsultarPorPontoDeSensoriamentoId(int pontoDeSensoriamento)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ponto_sensoriamento_id", pontoDeSensoriamento),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_sensoriamentoAtual_por_pontoDeSensoriamentoId", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaEntidadePadrao(tabela.Rows[0]);
        }


        protected override SqlParameter[] CriaParametros(SensoriamentoAtual sensoriamentoAtual)
        {
            var sensoriamentoAtualParametros = new List<SqlParameter>
            {
                new SqlParameter("ponto_sensoriamento_id", sensoriamentoAtual.PontoDeSensoriamentoId),
                new SqlParameter("nivel_pluviosidade", sensoriamentoAtual.NivelPluviosidade),
                new SqlParameter("vazao_da_agua", sensoriamentoAtual.VazaoDaAgua),
                new SqlParameter("altura_agua", sensoriamentoAtual.AlturaAgua)
            };
            if (sensoriamentoAtual.Id > 0)
                sensoriamentoAtualParametros.Add(new SqlParameter("id", sensoriamentoAtual.Id));

            return sensoriamentoAtualParametros.ToArray();
        }

        protected override SensoriamentoAtual MontaEntidadePadrao(DataRow registro)
        {
            var pontoDeSensoriamento = new SensoriamentoAtual
            {
                Id = Convert.ToInt32(registro["id"]),
                PontoDeSensoriamentoId = Convert.ToInt32(registro["ponto_sensoriamento_id"]),
                NivelPluviosidade = Convert.ToDouble(registro["nivel_pluviosidade"]),
                VazaoDaAgua = Convert.ToDouble(registro["vazao_agua"]),
                AlturaAgua = Convert.ToDouble(registro["altura_agua"]),
                UltimaAtualizacao = Convert.ToDateTime(registro["ultima_atualizacao"]),
                TipoRisco = (ETipoRisco)Convert.ToInt32(registro["tipo_risco"])
            };

            return pontoDeSensoriamento;
        }

    }
}
