using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels
{
    public class CriarEditarPontoDeSensoriamentoViewModel : BaseCadastroEdicaoViewModel
    {
        [DisplayName("Helix Id")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(100)]
        public string HelixId { get; set; }
        public bool Ativo { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string CodigoEstado { get; set; }

        [DisplayName("Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string CodigoCidade { get; set; }

        [DisplayName("Latitude")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Latitude { get; set; }

        [DisplayName("Longitude")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Longitude { get; set; }

        public CriarEditarPontoDeSensoriamentoViewModel(PontoDeSensoriamento pontoDeSensoriamento)
        {
            HelixId = pontoDeSensoriamento.HelixId.Replace("urn:ngsi-ld:sensor:", "");
            Ativo = pontoDeSensoriamento.AtivoHelix;
            CodigoEstado = pontoDeSensoriamento.EstadoAtendido.CodigoEstado;
            CodigoCidade = pontoDeSensoriamento.CidadeAtendida.CodigoCidade;
            Latitude = pontoDeSensoriamento.Latitude.ToString();
            Longitude = pontoDeSensoriamento.Longitude.ToString();
            CodigoCidade = pontoDeSensoriamento.CidadeAtendida.CodigoCidade;
        }

        public CriarEditarPontoDeSensoriamentoViewModel()
        {
        }
    }
}
