using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig
{
    public abstract class BaseDAO<T> where T : BaseEntity
    {
        public BaseDAO()
        {
            SetTabela();
        }

        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "sp_listar";
        protected abstract SqlParameter[] CriaParametros(T model);
        protected abstract T MontaEntidadePadrao(DataRow registro);
        protected abstract void SetTabela();

        public virtual void Inserir(T model)
        {
            HelperDAO.ExecutaProc("sp_insert_" + Tabela, CriaParametros(model));
        }

        public virtual void Atualizar(T model)
        {
            HelperDAO.ExecutaProc("sp_update_" + Tabela, CriaParametros(model));
        }

        public virtual void Deletar(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };

            HelperDAO.ExecutaProc("sp_delete", p);
        }

        public virtual T ConsultarPorId(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaEntidadePadrao(tabela.Rows[0]);
        }

        public virtual List<T> Listar()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);

            List<T> lista = new List<T>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }
    }
}
