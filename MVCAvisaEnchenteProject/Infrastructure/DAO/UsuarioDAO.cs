using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
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
    public class UsuarioDAO : BaseDAO<Usuario>
    {
        protected override void SetTabela()
        {
            Tabela = "usuarios";
        }

        public void RegistrarUsuario(Usuario usuario)
        {        
            HelperDAO.ExecutaProc("sp_Registrar_Usuario", CriaParametros(usuario));
        }
        public Usuario LogarUsuario(string email, string senha)
        {
            SqlParameter[] parametros = {
                new SqlParameter("email", email),
                new SqlParameter("senha", senha),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_Login_Usuario", parametros);

            if (tabela.Rows.Count != 0)
                return MontaEntidadePadrao(tabela.Rows[0]);
            else
                return null;
        }

        public void DefineCidadeUsuario(int usuarioId, int cidadeId)
        {
            SqlParameter[] parametros = {
                new SqlParameter("usuarioId", usuarioId),
                new SqlParameter("cidadeId", cidadeId),
            };

            HelperDAO.ExecutaProc("sp_Define_Cidade_Usuario", parametros);
        }

        public bool EmailJaExiste(string email)
        {
            SqlParameter[] emailParametro = {
                new SqlParameter("email", email),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_Consulta_Usuario_Por_Email", emailParametro);

            if (tabela.Rows.Count == 0)
                return false;
            else
                return true;
        }


        #region Helpers

        protected override Usuario MontaEntidadePadrao(DataRow registro)
        {
            var usuario = new Usuario
            {
                Id = Convert.ToInt32(registro["id"]),
                NomeCompleto = registro["nome_completo"].ToString(),
                Email = registro["email"].ToString(),
                Senha = registro["senha"].ToString(),
                TipoUsuario = (ETipoUsuario)Convert.ToInt32(registro["tipo_usuario"]),
                PrimeiroLogin = Convert.ToBoolean(registro["primeiro_login"]),
            };
            if (registro["cidade_atendida_id"] != DBNull.Value)
                usuario.CidadeAtendidaId = Convert.ToInt32(registro["cidade_atendida_id"]);
            

            return usuario;
        }


        protected override SqlParameter[] CriaParametros(Usuario usuario)
        {
            SqlParameter[] usuarioParametros = {
                new SqlParameter("nome_completo", usuario.NomeCompleto),
                new SqlParameter("email", usuario.Email),
                new SqlParameter("senha", usuario.Senha),
                new SqlParameter("tipo_usuario", usuario.TipoUsuario),
                new SqlParameter("primeiro_login", usuario.PrimeiroLogin),
            };

            return usuarioParametros;
        }
        #endregion
    }
}
