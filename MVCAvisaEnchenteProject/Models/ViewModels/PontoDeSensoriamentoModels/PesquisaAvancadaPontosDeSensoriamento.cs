﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Models.ViewModels.PontoDeSensoriamentoModels
{
    public class PesquisaAvancadaPontosDeSensoriamento
    {
        [DisplayName("Helix Id")]
        [MaxLength(100)]
        public string HelixId { get; set; }

        [DisplayName("Ativo")]
        public bool? Ativo { get; set; }

        [DisplayName("Última Edição Por")]
        public int? UsuarioId { get; set; }

        [DisplayName("Estado")]
        public int? EstadoId { get; set; }

        [DisplayName("Cidade")]
        public int? CidadeId { get; set; }

        public SelectList UsuariosAdmin { get; set; }
        public SelectList Estados { get; set; }

        public PesquisaAvancadaPontosDeSensoriamento(SelectList selectEstados, SelectList selectUsuarios)
        {
            Estados = selectEstados;
            UsuariosAdmin = selectUsuarios;
        }

        public PesquisaAvancadaPontosDeSensoriamento(SelectList selectEstados, SelectList selectUsuarios, int estadoId, int usuarioId)
        {
            Estados = selectEstados;
            UsuariosAdmin = selectUsuarios;
            EstadoId = estadoId;
            UsuarioId = usuarioId;
        }

        public PesquisaAvancadaPontosDeSensoriamento()
        {

        }
    }
}
