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
    public class ParametroNotificacaoDAO : BaseDAO<ParametroNotificacao>
    {
        protected override void SetTabela()
        {
            Tabela = "notificacoes_parametros";
        }

        protected override SqlParameter[] CriaParametros(ParametroNotificacao parametroNotificacao)
        {
            var pontoDeSensoriamentoParametros = new List<SqlParameter>
            {
                new SqlParameter("ponto_sensoriamento_id", parametroNotificacao.PontoDeSensoriamentoId),
                new SqlParameter("tipo_risco", parametroNotificacao.TipoRisco),
                new SqlParameter("vazao_agua", parametroNotificacao.VazaoDaAgua),
                new SqlParameter("nivel_pluviosidade", parametroNotificacao.NivelPluviosidade),
                new SqlParameter("altura_agua", parametroNotificacao.AlturaAgua)
            };
            if (parametroNotificacao.Id > 0)
                pontoDeSensoriamentoParametros.Add(new SqlParameter("id", parametroNotificacao.Id));

            return pontoDeSensoriamentoParametros.ToArray();
        }

        protected override ParametroNotificacao MontaEntidadePadrao(DataRow registro)
        {
            throw new NotImplementedException();
        }

    }
}
