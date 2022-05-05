using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.CustomAttributes;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.Request;
using MVCAvisaEnchenteProject.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Controllers
{
    public class UsuarioController : BaseController<Usuario, UsuarioDAO>
    {
        public UsuarioController() : base()
        {
            
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Index()
        {
            return base.Index();
        }

        [HttpDelete]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public override IActionResult Deletar(int id)
        {
            if(id == Convert.ToInt32(ObterIdUsuarioLogado()))
                return Json(new JsonFormResponse(messageErro: "Você não pode excluir seu proprio Usuário!"));

            return base.Deletar(id);
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult CriarOuEditarUsuario(int id = 0)
        {   
            if(id == 0)
                return View(new AdminCriarEditarUsuarioViewModel());

            var usuario = DAOPrincipal.ConsultarPorId(id);
            if(usuario != null)
                return View(new AdminCriarEditarUsuarioViewModel(usuario));

            TempData["Error"] = "Usuário não existe!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> CriarOuEditarUsuario(int id, AdminCriarEditarUsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(!usuarioViewModel.EdicaoModel() && string.IsNullOrEmpty(usuarioViewModel.Senha))
                    {
                        ModelState.AddModelError("Senha", "A senha é Obrigatória!");
                        return Json(new JsonFormResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "CriarOuEditarUsuario", usuarioViewModel)));
                    }

                    var usuario = new Usuario(usuarioViewModel);
                    if (id == 0)
                    {
                        if (!DAOPrincipal.EmailJaExiste(usuarioViewModel.Email))               
                            DAOPrincipal.Inserir(usuario);                 
                        else
                            return Json(new JsonFormResponse(messageErro: "Email já Existe!"));                   
                    }
                    else
                    {
                        if(DAOPrincipal.ConsultarPorId(id) != null)
                            DAOPrincipal.AtualizarUsuarioAdmin(usuarioViewModel);
                        else
                            return Json(new JsonFormResponse(messageErro: "Usuário não encontrado!"));
                    }

                    return Json(new JsonFormResponse(valido: true));
                }
                catch (Exception)
                {
                    return Json(new JsonFormResponse(messageErro: "Ocorreu um erro ao tentar salvar o usuário!"));
                }
            }
            return Json(new JsonFormResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "CriarOuEditarUsuario", usuarioViewModel)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public async Task<IActionResult> PesquisaAvancadaUsuarios(PesquisaAvancadaUsuariosViewModel pesquisaAvancadaUsuarios)
        {
            try
            {
                return Json(new JsonFormResponse(valido: true, html: HelperRenderRazorView.RenderRazorViewToString(this, "_ListarUsuarios", DAOPrincipal.PesquisaAvancadaUsuarios(pesquisaAvancadaUsuarios))));
            }
            catch (Exception e)
            {
                return Json(new JsonFormResponse(messageErro: "Ocorreu um erro ao pesquisar os Usuários!"));
            }   
        }

        [HttpGet]
        [Authorize]
        public IActionResult MeuPerfil()
        {
            return View(ObterUsuarioLogado() ?? new Usuario());
        }
    }
}
