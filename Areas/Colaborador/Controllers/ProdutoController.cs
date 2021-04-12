using LojaVirtual.Libraries.Arquivo;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;
        private IImagemRepository _imagemRepository;
        public ProdutoController(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IImagemRepository imagemRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _imagemRepository = imagemRepository;
        }
        public IActionResult Index(int? pagina, string pesquisa)
        {
            var produtos = _produtoRepository.ObterTodosProdutos(pagina, pesquisa);
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto produto)
        {
            if (ModelState.IsValid)
            {
                // salvar as informações do produto no banco de dados
                _produtoRepository.Cadastrar(produto);

                //CaminhoTempo -> Mover a imagem para o caminho definitivo
                List<Imagem> ListaImagensDef = GerenciadorArquivo.MoverImagensProduto(
                    new List<string>(Request.Form["imagem"]), //caminhos temporários das imagens
                    produto.Id
                    );

                //Salvar o caminho definitivo de todas as imagens e salvar no banco de dados
                _imagemRepository.CadastrarImagens(ListaImagensDef, produto.Id);

                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                    .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
                // Retornar as listas de imagens para a tela quando houver problema na validação das informações
                produto.Imagens = new List<string>(Request.Form["imagem"])
                    .Where(a => a.Trim().Length > 0) // pegar somente as imagens com conteudo
                    .Select(a => new Imagem() { Caminho = a }).ToList();
                return View(produto);
            }
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            Produto produto = _produtoRepository.ObterProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult Atualizar(Produto produto, int id)
        {
            if (ModelState.IsValid)
            {
                _produtoRepository.Atualizar(produto);
                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View(produto);
        }
    }
}
// Continuar módulo 15 aula 40