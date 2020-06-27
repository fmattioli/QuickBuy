using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickBuy.Dominio.Contratos;
using QuickBuy.Dominio.Entidades;
using System;
using QuickBuy.Web.Services.Auth;
using QuickBuy.Web.ViewModel;

namespace QuickBuy.Web.Controllers
{
    [Route("api/[Controller]")]
    public class UsuarioController : Controller
    {
        
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost("VerificarUsuario")]
        [AllowAnonymous]
        public ActionResult VerificarUsuario([FromServices] IUsuarioRepositorio usuarioRepositorio, [FromServices] IAssistente assistente, [FromBody] Usuario usuario)
        {
            try
            {
                var usuarioRetorno = usuarioRepositorio.Obter(usuario.Email, usuario.Senha);
                if (usuarioRetorno != null)
                {
                    var token = Token.GenerateToken(usuarioRetorno, assistente.Key);
                    var usuarioViewModel = new UsuarioViewModel
                    {
                        Email = usuarioRetorno.Email,
                        Id = usuarioRetorno.Id,
                        Nivel = usuarioRetorno.Nivel,
                        Nome = usuarioRetorno.Nome,
                        Senha = usuarioRetorno.Senha,
                        SobreNome = usuarioRetorno.SobreNome,
                        Token = token
                    };
                    return Ok(usuarioViewModel);
                }
                return BadRequest("Usuario ou senha invalidos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Post([FromServices] IUsuarioRepositorio usuarioRepositorio, [FromBody] Usuario usuario)
        {
            try
            {
                var usuarioCadastrado = usuarioRepositorio.Obter(usuario.Email);
                if(usuarioCadastrado != null)
                {
                    return BadRequest("Usuario já existe sistema!!!");
                }
                usuarioRepositorio.Adicionar(usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        

    }
}
