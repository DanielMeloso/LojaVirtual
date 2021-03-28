using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class CategoriaController : Controller
    {
        private ICategoriaRepository _categoriaRepository;
        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public IActionResult Index(int? pagina)
        {
            var categorias = _categoriaRepository.ObterTodasCategorias(pagina);
            /*
             * Paginação foi feita dentro do contract da categoria, assim, o db não realizará a busca no banco inteiro,
             * somente da quantidade que será exibida
             */
            //IPagedList<Categoria> categoriaPaginada = categorias.ToPagedList(1, 25);
            return View(categorias);
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            // carregar as informações das categorias para listar no drop
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Cadastrar(categoria);
                TempData["MSG_S"] = "Registro salvo com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a => new SelectListItem(a.Nome, a.Id.ToString() ));
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int Id)
        {
            var categoria = _categoriaRepository.ObterCategoria(Id);
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Where(a => a.Id != Id) // Não listar a mesma categoria do item que está sendo alterado
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm]Categoria categoria, int Id)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Atualizar(categoria);
                TempData["MSG_S"] = "Registro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias()
                .Where(a => a.Id != Id) // Não listar a mesma categoria do item que está sendo alterado
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int Id)
        {
            _categoriaRepository.Excluir(Id);
            TempData["MSG_S"] = "Registro Excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
