using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        const int RegistroPorPagina = 10; // quantidade de registros que será consultado no banco de dados a cada requisição
        LojaVirtualContext _banco;

        public CategoriaRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }
        public void Atualizar(Categoria categoria)
        {
            _banco.Update(categoria);
            _banco.SaveChanges();
        }

        public void Cadastrar(Categoria categoria)
        {
            _banco.Add(categoria);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Categoria categoria = ObterCategoria(Id);
            _banco.Remove(categoria);
            _banco.SaveChanges();
        }

        public Categoria ObterCategoria(int Id)
        {
            return _banco.Categorias.Find(Id);
        }

        public Categoria ObterCategoria(string slug)
        {
            return _banco.Categorias.Where(a => a.Slug == slug).FirstOrDefault();
        }

        private List<Categoria> Categorias;
        private List<Categoria> listaCategoriaRecursiva = new List<Categoria>();
        public IEnumerable<Categoria> ObterCategoriasRecursivas(Categoria categoriaPai)
        {
            // para que não seja obtido todas as categorias sempre que a recursividade seja executada
            if (Categorias == null)
            {
                Categorias = ObterTodasCategorias().ToList();
            }

            if (!listaCategoriaRecursiva.Exists(a => a.Id == categoriaPai.Id))
            {
                listaCategoriaRecursiva.Add(categoriaPai);
            }
            var listaCategoriaFilho = Categorias.Where(a => a.CategoriaPaiId == categoriaPai.Id);
            if (listaCategoriaFilho.Count() > 0)
            {
                listaCategoriaRecursiva.AddRange(listaCategoriaFilho.ToList());
                foreach (var cat in listaCategoriaFilho)
                {
                    ObterCategoriasRecursivas(cat);
                }
            }

            return listaCategoriaRecursiva;
        }

        public IPagedList<Categoria> ObterTodasCategorias(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return _banco.Categorias.Include(a => a.CategoriaPai).ToPagedList<Categoria>(NumeroPagina, RegistroPorPagina);
        }

        public IEnumerable<Categoria> ObterTodasCategorias()
        {
            return _banco.Categorias;
        }
    }
}
