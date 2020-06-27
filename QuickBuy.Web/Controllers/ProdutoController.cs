using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickBuy.Dominio.Contratos;
using QuickBuy.Dominio.Entidades;

namespace QuickBuy.Web.Controllers
{
    [Route("api/[Controller]")]
    public class ProdutoController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromServices] IProdutoRepositorio produtoRepositorio, [FromHeader] string Authorization)
        {

            try
            {
                return Json(produtoRepositorio.ObterTodos());
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromServices] IProdutoRepositorio produtoRepositorio,
                                    [FromBody] Produto produto, [FromServices] IHttpContextAccessor httpContextAccessor, [FromHeader] string Authorization)
        {
            try
            {
                var x = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                //var formFile = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                produto.Validate();
                if (!produto.EhValido)
                {
                    return BadRequest(produto.ObterMensagensValidacao());
                }
                if (produto.Id > 0)
                {
                    produtoRepositorio.Atualizar(produto);
                }
                else
                {
                    produtoRepositorio.Adicionar(produto);
                }

                return Created("api/produto", produto);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }

        [HttpPut("atualizarProduto")]
        [Authorize]
        public IActionResult Atualizar([FromServices] IProdutoRepositorio produtoRepositorio,
                                    [FromBody] Produto produto, [FromServices] IHttpContextAccessor httpContextAccessor, [FromHeader] string Authorization)
        {
            try
            {
                var x = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                //var formFile = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                produto.Validate();
                if (!produto.EhValido)
                {
                    return BadRequest(produto.ObterMensagensValidacao());
                }

                if (produto.Id > 0)
                {
                    produtoRepositorio.Atualizar(produto);
                }

                return Created("api/produto", produto);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }

        [HttpPost("Deletar")]
        [Authorize(Roles = "Gerente")]
        public IActionResult Deletar([FromServices] IProdutoRepositorio produtoRepositorio, [FromBody] Produto produto)
        {
            try
            {
                produtoRepositorio.Remover(produto);
                return Json(produtoRepositorio.ObterTodos());
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }
        [HttpPost("EnviarArquivo")]
        [Authorize]
        public IActionResult EnviarArquivo([FromServices] IHttpContextAccessor httpContextAccessor, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var formFile = httpContextAccessor.HttpContext.Request.Form.Files["arquivoEnviado"];
                var nomeArquivo = formFile.FileName;
                var extensao = nomeArquivo.Split(".").Last();
                string novoNomeArquivo = GerarNovoNomeArquivo(nomeArquivo, extensao);
                var pastaArquivos = hostingEnvironment.WebRootPath + @"\arquivos\";
                var nomeCompleto = pastaArquivos + novoNomeArquivo;
                using (var streamArquivo = new FileStream(nomeCompleto, FileMode.Create))
                {
                    formFile.CopyTo(streamArquivo);
                }

                return Json(novoNomeArquivo);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }



        private static string GerarNovoNomeArquivo(string nomeArquivo, string extensao)
        {
            var arrayNomeCompacto = Path.GetFileNameWithoutExtension(nomeArquivo).Take(10).ToArray();
            var novoNomeArquivo = new string(arrayNomeCompacto).Replace(" ", "");
            novoNomeArquivo = $"{novoNomeArquivo}_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.{extensao}";
            return novoNomeArquivo;
        }
    }
}
