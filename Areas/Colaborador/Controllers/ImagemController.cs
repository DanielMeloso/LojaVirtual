using LojaVirtual.Libraries.Arquivo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    /*
     * Controlador responsável por controlar imagens dos produtos
     * Envio realizado utilizando Ajax. Assim que escolher imagem o upload é inicializado.
     */
    [Area("Colaborador")]
    public class ImagemController : Controller
    {
        [HttpPost]
        public IActionResult Armazenar(IFormFile file)
        {
            var Caminho = GerenciadorArquivo.ArmazenarImagemProduto(file);

            if (Caminho.Length > 0)
            {
                // Status HTTP - 200 (OK)
                return Ok(new { caminho = Caminho }); //JSON -> JavaScript
            }
            else
            {
                return new StatusCodeResult(500); // Erro interno do servidor
            }
        }

        public IActionResult Deletar(string caminho)
        {
            if (GerenciadorArquivo.DeletarImagemProduto(caminho))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
