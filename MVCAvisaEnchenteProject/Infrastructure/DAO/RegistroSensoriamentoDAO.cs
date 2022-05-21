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
    public class RegistroSensoriamentoDAO : BaseDAO<RegistroSensoriamento>
    {
        protected override void SetTabela()
        {
            Tabela = "registros_sensoriamento";
        }

        public RegistroSensoriamento ConsultarPorPontoDeSensoriamentoId(int pontoDeSensoriamento)
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

        public List<RegistroSensoriamento> ListarSensoriamentoAtualPorCidade(int cidadeId)
        {
            SqlParameter[] cidadeIdParametro = {
                new SqlParameter("cidade_atendida_id", cidadeId),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_listar_sensoariamento_atual_por_cidade", cidadeIdParametro);

            var lista = new List<RegistroSensoriamento>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaSensoriamentoAtualComPontoDeSensoriamento(registro));

            return lista;
        }

        protected override SqlParameter[] CriaParametros(RegistroSensoriamento sensoriamentoAtual)
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

        protected override RegistroSensoriamento MontaEntidadePadrao(DataRow registro)
        {
            var pontoDeSensoriamento = new RegistroSensoriamento
            {
                Id = Convert.ToInt32(registro["id"]),
                PontoDeSensoriamentoId = Convert.ToInt32(registro["ponto_sensoriamento_id"]),
                NivelPluviosidade = Convert.ToDouble(registro["nivel_pluviosidade"]),
                VazaoDaAgua = Convert.ToDouble(registro["vazao_agua"]),
                AlturaAgua = Convert.ToDouble(registro["altura_agua"]),
                DataRegistro = Convert.ToDateTime(registro["data_registro"]),
                TipoRisco = (ETipoRisco)Convert.ToInt32(registro["tipo_risco"])
            };

            return pontoDeSensoriamento;
        }

        protected RegistroSensoriamento MontaSensoriamentoAtualComPontoDeSensoriamento(DataRow registro)
        {
            var sensoriamento = new RegistroSensoriamento
            {
                Id = Convert.ToInt32(registro["id"]),
                PontoDeSensoriamentoId = Convert.ToInt32(registro["ponto_sensoriamento_id"]),
                NivelPluviosidade = Convert.ToDouble(registro["nivel_pluviosidade"]),
                VazaoDaAgua = Convert.ToDouble(registro["vazao_agua"]),
                AlturaAgua = Convert.ToDouble(registro["altura_agua"]),
                DataRegistro = Convert.ToDateTime(registro["data_registro"]),
                TipoRisco = (ETipoRisco)Convert.ToInt32(registro["tipo_risco"]),
                PontoDeSensoriamento = new PontoDeSensoriamento
                {
                    Id = Convert.ToInt32(registro["ponto_sensoriamento_id"]),
                    HelixId = (registro["helix_id"]).ToString(),
                    Latitude = Convert.ToDecimal(registro["latitude"]),
                    Longitude = Convert.ToDecimal(registro["longitude"]),
                    CidadeAtendidaId = Convert.ToInt32(registro["cidade_atendida_id"])
                }
            };

            return sensoriamento;
        }

        public override void Atualizar(RegistroSensoriamento model)
        {
            throw new NotImplementedException("Esse metodo não foi implementado");
        }

        //public override List<RegistroSensoriamento> Listar()
        //{
        //    var tabela = HelperDAO.ExecutaProcSelect("sp_listar_" + Tabela, Array.Empty<SqlParameter>());

        //    List<RegistroSensoriamento> lista = new List<RegistroSensoriamento>();

        //    foreach (DataRow registro in tabela.Rows)
        //        lista.Add(MontaSensoriamentoAtualComPontoDeSensoriamento(registro));

        //    return lista;
        //}
    }
}
