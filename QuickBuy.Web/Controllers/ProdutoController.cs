using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IProdutoRepositorio _produtoRepositorio;
        private IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public ProdutoController(IProdutoRepositorio produtoRepositorio, IHttpContextAccessor iHttpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            this._produtoRepositorio = produtoRepositorio;
            this._httpContextAccessor = iHttpContextAccessor;
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
       {
           
            try
            {
                return Json(_produtoRepositorio.ObterTodos());
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Produto produto)
        {
            try
            {
                var formFile = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                produto.Validate();
                if (!produto.EhValido)
                {
                    return BadRequest(produto.ObterMensagensValidacao()); 
                }
                if(produto.Id > 0)
                {
                    _produtoRepositorio.Atualizar(produto);
                }
                else
                {
                    _produtoRepositorio.Adicionar(produto);
                }
                
                return Created("api/produto", produto);
            }
            catch(Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }

        [HttpPost("Deletar")]
        public IActionResult Deletar([FromBody] Produto produto)
        {
            try
            {
                _produtoRepositorio.Remover(produto);
                return Json(_produtoRepositorio.ObterTodos());
            }
            catch (Exception erro)
            {
                return BadRequest(erro.ToString());
            }
        }
        [HttpPost("EnviarArquivo")]
        public IActionResult EnviarArquivo()
        {
            try
            {
                var formFile = _httpContextAccessor.HttpContext.Request.Form.Files["arquivoEnviado"];
                var nomeArquivo = formFile.FileName;
                var extensao = nomeArquivo.Split(".").Last();
                string novoNomeArquivo = GerarNovoNomeArquivo(nomeArquivo, extensao);
                var pastaArquivos = _hostingEnvironment.WebRootPath + @"\arquivos\";
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
