using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        /*
         * SLUG: 
         * Nome: Telefone sem fio
         * Slug: telefone-sem-fio
         * 
         * www.lojavirtual.com.br/categoria/1 (URL Normal)
         * www.lojavirtual.com.br/categoria/informatica (URL amigável e com Slug)
         * 
         */
        public string Slug { get; set; }

        /*
         * Auto-relacionamento (niveis da categoria)
         * - Informatica
         * --Mouse
         * ---Mouse sem fio
         * ---Mouse Gamer
         */
        public int? CategoriaPaiId { get; set; }

        /*
         * ORM - EntityFrameworkCore
         */
        [ForeignKey("CategoriaPaiId")]
        public virtual Categoria CategoriaPai { get; set; }
    }
}
