using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels
{
    public class AtualizarImagemPerfilViewModel : BaseCadastroEdicaoViewModel
    {
        public IFormFile Imagem { get; set; }
        public byte[] ImagemEmByte { get; set; }
        public string ImagemEmBase64
        {
            get
            {
                if (ImagemEmByte != null)
                    return Convert.ToBase64String(ImagemEmByte);
                else
                    return string.Empty;
            }
        }
    }
}
