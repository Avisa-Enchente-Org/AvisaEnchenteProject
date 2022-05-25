using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAvisaEnchenteProject.Infrastructure.CustomAttributes;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Infrastructure.Helpers;
using MVCAvisaEnchenteProject.Models.Entidades;
using MVCAvisaEnchenteProject.Models.Enum;
using MVCAvisaEnchenteProject.Models.ViewModels;
using MVCAvisaEnchenteProject.Models.ViewModels.UsuarioModels;
using System;
using System.Collections.Generic;
using System.IO;
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
        public override IActionResult Index(int id)
        {
            return base.Index(id);
        }

        [HttpDelete]
        [Authorize]
        public override IActionResult Deletar(int id)
        {
            base.Deletar(id);

            if (id == Convert.ToInt32(ObterIdUsuarioLogado()))
                return RedirectToAction("Logout", "Conta");

            return Json(new JsonResponse());
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
        public IActionResult SalvarUsuario(int id, [Bind("Id, NomeCompleto, Email, Senha, TipoUsuario")] AdminCriarEditarUsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(!usuarioViewModel.EdicaoModel() && string.IsNullOrEmpty(usuarioViewModel.Senha))
                    {
                        ModelState.AddModelError("Senha", "A senha é Obrigatória!");
                        return Json(new JsonResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "CriarOuEditarUsuario", usuarioViewModel)));
                    }

                    var usuario = new Usuario(usuarioViewModel);
                    if (id == 0)
                    {
                        if (!DAOPrincipal.EmailJaExiste(usuarioViewModel.Email))               
                            DAOPrincipal.Inserir(usuario);                 
                        else
                            return Json(new JsonResponse(messageErro: "Email já Existe!"));                   
                    }
                    else
                    {
                        if(DAOPrincipal.ConsultarPorId(id) != null)
                            DAOPrincipal.AtualizarUsuarioAdmin(usuarioViewModel);
                        else
                            return Json(new JsonResponse(messageErro: "Usuário não encontrado!"));
                    }

                    return Json(new JsonResponse(valido: true));
                }
                catch (Exception e)
                {
                    return Json(new JsonResponse(messageErro: "Ocorreu um erro ao tentar salvar o usuário!"));
                }
            }
            return Json(new JsonResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "CriarOuEditarUsuario", usuarioViewModel)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult PesquisaAvancadaUsuarios(PesquisaAvancadaUsuariosViewModel pesquisaAvancadaUsuarios)
        {
            try
            {
                return Json(new JsonResponse(valido: true, html: HelperRenderRazorView.RenderRazorViewToString(this, "_ListarUsuarios", DAOPrincipal.PesquisaAvancadaUsuarios(pesquisaAvancadaUsuarios))));
            }
            catch (Exception e)
            {
                return Json(new JsonResponse(messageErro: "Ocorreu um erro ao pesquisar os Usuários!"));
            }   
        }

        [HttpGet]
        [Authorize(Roles = nameof(ETipoUsuario.Admin))]
        public IActionResult ObtemSelectListUsuariosAdmin()
        {
            var usuarios = DAOPrincipal.ListarUsuariosAdministradores();
            List<SelectListItem> selectUsuarios = new List<SelectListItem>();
            usuarios.ToList().ForEach(x =>
            {
                selectUsuarios.Add(new SelectListItem { Text = x.NomeCompleto, Value = x.Id.ToString() });
            });

            return Ok(selectUsuarios);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditarDadosPessoais(int id = 0)
        {
            var usuario = DAOPrincipal.ConsultarPorId(id);
            if (usuario != null)
                return View(new EditarDadosPessoaisViewModel(usuario));

            TempData["Error"] = "Usuário não existe!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditarLocalizacao(int id = 0)
        {
            var usuario = DAOPrincipal.ConsultarEnderecoUsuario(id);
            if (usuario != null)
                return View(new EditarLocalizacaoViewModel(usuario));

            TempData["Error"] = "Usuário não existe!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult SalvarDadosPessoais(int id, [Bind("Id, NomeCompleto, Email, Senha")] EditarDadosPessoaisViewModel dadosPessoaisViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = DAOPrincipal.ConsultarPorId(id);
                    if (usuario != null)
                    {
                        usuario.Email = dadosPessoaisViewModel.Email;
                        usuario.NomeCompleto = dadosPessoaisViewModel.NomeCompleto;
                        if(dadosPessoaisViewModel.Senha != null)
                            usuario.Senha = dadosPessoaisViewModel.Senha;

                        DAOPrincipal.Atualizar(usuario);
                    }
                    else
                        return Json(new JsonResponse(messageErro: "Usuário não encontrado!"));
                  

                    return Json(new JsonResponse(valido: true));
                }
                catch (Exception e)
                {
                    return Json(new JsonResponse(messageErro: "Ocorreu um erro ao tentar salvar os dados pessoais!"));
                }
            }
            return Json(new JsonResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "EditarDadosPessoais", dadosPessoaisViewModel)));
        }

        [HttpPost]
        [Authorize]
        public IActionResult SalvarLocalizacao(int id, [Bind("Id, EstadoAtendidoId, CidadeAtendidaId")] EditarLocalizacaoViewModel localizacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = DAOPrincipal.ConsultarPorId(id);
                    if (usuario != null)
                    {
                        DAOPrincipal.DefineCidadeUsuario(id, localizacaoViewModel.CidadeAtendidaId);
                    }
                    else
                        return Json(new JsonResponse(messageErro: "Usuário não encontrado!"));


                    return Json(new JsonResponse(valido: true));
                }
                catch (Exception e)
                {
                    return Json(new JsonResponse(messageErro: "Ocorreu um erro ao tentar salvar a Localização!"));
                }
            }
            return Json(new JsonResponse(valido: false, html: HelperRenderRazorView.RenderRazorViewToString(this, "EditarLocalizacao", localizacaoViewModel)));
        }


        [HttpPost]
        [Authorize]
        public IActionResult SalvarImagemDePerfil(int id, AtualizarImagemPerfilViewModel imagemPerfilViewModel)
        {
            try
            {
                var usuario = DAOPrincipal.ConsultarPorId(id);
                if (usuario != null)
                {
                    imagemPerfilViewModel.ImagemEmByte = ConvertImageToByte(imagemPerfilViewModel.Imagem);
                    if (imagemPerfilViewModel.ImagemEmByte == null)
                    {
                        TempData["Error"] = "Imagem Inválida!";
                        return RedirectToAction("MeuPerfil");
                    }


                    DAOPrincipal.AtualizaImagemDePerfil(imagemPerfilViewModel);
                    HttpContext.Session.SetString("ImagemPerfilBase64", DAOPrincipal.ConsultarPorId(id).ImagemDePerfilEmBase64);
                }
                return RedirectToAction("MeuPerfil");
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel(e.ToString()));
            }

        }


        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        [HttpGet]
        [Authorize]
        public IActionResult MeuPerfil()
        { 
            return View(ObterUsuarioLogado() ?? new Usuario());
        }
    }
}
