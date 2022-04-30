﻿using Microsoft.AspNetCore.Mvc;
using MVCAvisaEnchenteProject.Infrastructure.DAO.DAOConfig;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public abstract class BaseController<T, D> : Controller where T : BaseEntity where D : BaseDAO<T>
    {
        protected D DAOPrincipal { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "form";

        public virtual IActionResult Index()
        {
            try
            {
                var lista = DAOPrincipal.Listar();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected bool UsuarioEstaLogado() => !string.IsNullOrEmpty(User?.FindFirst("UsuarioId")?.Value);
        protected string ObterIdUsuarioLogado() => User?.FindFirst("UsuarioId")?.Value;
    }
}
