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
            if (registro.Table.Columns.Contains("helix_id"))
            { 
                pontoDeSensoriamento.PontoDeSensoriamento = new PontoDeSensoriamento
                {
                    HelixId = (registro["helix_id"]).ToString()
                };
            }

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

        public List<RegistroSensoriamento> ConsultaUltimosRegistrosSensoriamento(int pdsId)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ponto_sensoriamento_id", pdsId),
            };
            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_ultimos_50_registros_sensoriamento_por_pontoDeSensoriamentoId", p);

            List<RegistroSensoriamento> lista = new List<RegistroSensoriamento>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        public List<RegistroSensoriamento> ConsultaUltimosAlertasDeRisco(int pdsId)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ponto_sensoriamento_id", pdsId),
            };
            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_ultimos_50_alertas_sensoriamento_por_pontoDeSensoriamentoId", p);

            List<RegistroSensoriamento> lista = new List<RegistroSensoriamento>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        public List<MediaDeSensoriamentoViewModel> ObterMediaDeSensoriamento(int pdsId)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ponto_sensoriamento_id", pdsId),
            };
            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_media_sensoriamento_de_15__porpontoDeSensoriamentoId", p);

            List<MediaDeSensoriamentoViewModel> lista = new List<MediaDeSensoriamentoViewModel>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaMediaSensoriamento(registro));

            return lista;
        }

        private MediaDeSensoriamentoViewModel MontaMediaSensoriamento(DataRow registro)
        {
            var media = new MediaDeSensoriamentoViewModel
            {
                MediaVazao = Convert.ToDouble(registro["media_vazao"]),
                MediaAltura = Convert.ToDouble(registro["media_altura"]),
                MediaChuva = Convert.ToDouble(registro["media_chuva"]),
                Dia = (registro["dia"]).ToString(),
            };
            if (media.Dia.Length == 1)
                media.Dia = "0" + media.Dia;

            return media;
        }

        public List<RegistroSensoriamento> PesquisaAvancadaAlertasRisco(PesquisaAvancadaAlertasRiscoViewModel pesquisaAvancadaAlertasRisco)
        {
            var p = new SqlParameter[]
            {
                    new SqlParameter("ponto_sensoriamento_id", pesquisaAvancadaAlertasRisco.PontoDeSensoriamentoId.HasValue && pesquisaAvancadaAlertasRisco.PontoDeSensoriamentoId.Value != default ? pesquisaAvancadaAlertasRisco.PontoDeSensoriamentoId.Value.ToString() : ""),
                    new SqlParameter("tipo_risco", pesquisaAvancadaAlertasRisco.TipoRisco.HasValue && pesquisaAvancadaAlertasRisco.TipoRisco.Value != default ? pesquisaAvancadaAlertasRisco.TipoRisco.Value.ToString() : ""),
                    new SqlParameter("cidadeId", pesquisaAvancadaAlertasRisco.CidadeId.HasValue && pesquisaAvancadaAlertasRisco.CidadeId.Value != default ? pesquisaAvancadaAlertasRisco.CidadeId.Value.ToString() : ""),
                    new SqlParameter("estadoId", pesquisaAvancadaAlertasRisco.EstadoId.HasValue && pesquisaAvancadaAlertasRisco.EstadoId.Value != default ? pesquisaAvancadaAlertasRisco.EstadoId.Value.ToString() : ""),
            };
            var lista = new List<RegistroSensoriamento>();

            var tabela = HelperDAO.ExecutaProcSelect("sp_pesquisa_avancada_alertas_risco", p);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }
    }
}
