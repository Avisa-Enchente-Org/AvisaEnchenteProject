using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.SensoriamentoAtualModels
{
    public class IndexSensoriamentoAtualViewModel
    {
        public Usuario UsuarioLogado { get; set; }
        public string EnderecoUsuario { get; set; }

        public IndexSensoriamentoAtualViewModel(Usuario usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
            EnderecoUsuario = $"{usuarioLogado.CidadeAtendida.Descricao}, {usuarioLogado.EstadoAtendido.Descricao}";
        }
        public IndexSensoriamentoAtualViewModel()
        {

        }
    }
}
