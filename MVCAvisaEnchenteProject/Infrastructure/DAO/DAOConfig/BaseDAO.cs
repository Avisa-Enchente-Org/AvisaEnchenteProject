using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig
{
    public abstract class BaseDAO<T> where T : BaseEntity
    {
        public BaseDAO()
        {
            SetTabela();
        }

        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "sp_Listagem";
        protected abstract SqlParameter[] CriaParametros(T model);
        protected abstract T MontaEntidade(DataRow registro);
        protected abstract void SetTabela();

        public virtual void Insert(T model)
        {
            HelperDAO.ExecutaProc("sp_Insert_" + Tabela, CriaParametros(model));
        }

        public virtual void Update(T model)
        {
            HelperDAO.ExecutaProc("sp_Update_" + Tabela, CriaParametros(model));
        }

        public virtual void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };

            HelperDAO.ExecutaProc("sp_Delete", p);
        }

        public virtual T Consulta(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_Consulta", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaEntidade(tabela.Rows[0]);
        }

        public virtual List<T> Listagem()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);

            List<T> lista = new List<T>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidade(registro));

            return lista;
        }
    }
}
