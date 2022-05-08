using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels;
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

        public List<Usuario> ListarUsuariosAdministradores()
        {
            var tabela = HelperDAO.ExecutaProcSelect("sp_listar_usuarios_administradores", Array.Empty<SqlParameter>());

            var lista = new List<Usuario>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }
        public Usuario LogarUsuario(LoginUsuarioRequest login)
        {
            SqlParameter[] parametros = {
                new SqlParameter("email", login.Email),
                new SqlParameter("senha", login.Senha),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_login_usuario", parametros);

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

            HelperDAO.ExecutaProc("sp_define_cidade_usuario", parametros);
        }

        public bool EmailJaExiste(string email)
        {
            SqlParameter[] emailParametro = {
                new SqlParameter("email", email),
            };

            var tabela = HelperDAO.ExecutaProcSelect("sp_consulta_usuario_por_email", emailParametro);

            if (tabela.Rows.Count == 0)
                return false;
            else
                return true;
        }

        public void AtualizarUsuarioAdmin(AdminCriarEditarUsuarioViewModel usuarioViewModel)
        {
            SqlParameter[] parametros = {
                new SqlParameter("id", usuarioViewModel.Id),
                new SqlParameter("tipo_usuario", usuarioViewModel.TipoUsuario)
            };

            HelperDAO.ExecutaProc("sp_atualiza_usuario_admin", parametros);
        }

        public List<Usuario> PesquisaAvancadaUsuarios(PesquisaAvancadaUsuariosViewModel pesquisaAvancadaUsuariosViewModel)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("nome_completo", pesquisaAvancadaUsuariosViewModel.NomeCompleto ?? ""),
                new SqlParameter("email", pesquisaAvancadaUsuariosViewModel.Email ?? ""),
                new SqlParameter("tipo_usuario", pesquisaAvancadaUsuariosViewModel.TipoUsuario == 0 ? "": pesquisaAvancadaUsuariosViewModel.TipoUsuario.ToString())
            };

            var lista = new List<Usuario>();
            DataTable tabela = HelperDAO.ExecutaProcSelect("sp_pesquisa_avancada_usuarios", p);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEntidadePadrao(registro));

            return lista;
        }

        #region Helpers

        protected override Usuario MontaEntidadePadrao(DataRow registro)
        {
            var usuario = new Usuario
            {
                Id = Convert.ToInt32(registro["id"]),
                NomeCompleto = registro["nome_completo"].ToString(),
                Email = registro["email"].ToString(),
                TipoUsuario = (ETipoUsuario)Convert.ToInt32(registro["tipo_usuario"]),
                PrimeiroLogin = Convert.ToBoolean(registro["primeiro_login"]),
            };
            if (registro.Table.Columns.Contains("senha"))
                usuario.Senha = registro["senha"].ToString();
            if (registro["cidade_atendida_id"] != DBNull.Value)
                usuario.CidadeAtendidaId = Convert.ToInt32(registro["cidade_atendida_id"]);
            

            return usuario;
        }


        protected override SqlParameter[] CriaParametros(Usuario usuario)
        {
            var usuarioParametros = new List<SqlParameter> 
            {
                new SqlParameter("nome_completo", usuario.NomeCompleto),
                new SqlParameter("email", usuario.Email),
                new SqlParameter("senha", usuario.Senha),
                new SqlParameter("tipo_usuario", usuario.TipoUsuario),
                new SqlParameter("primeiro_login", usuario.PrimeiroLogin),
            };
            if (usuario.Id > 0)
                usuarioParametros.Add(new SqlParameter("id", usuario.Id));

            return usuarioParametros.ToArray();
        }
        #endregion
    }
}
