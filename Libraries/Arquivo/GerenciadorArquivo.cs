using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Arquivo
{
    public class GerenciadorArquivo
    {
        public static string ArmazenarImagemProduto(IFormFile file)
        {
            // armazenar imagem em uma pasta
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", NomeArquivo);

            using(var stream = new FileStream(Caminho, FileMode.Create))
            {
                // transferir conteudo do arquivo enviado para o arquivo que criamos
                file.CopyTo(stream);
            }

            // retornar para o JS o caminho do arquivo
            return Path.Combine("uploads/temp", NomeArquivo);
        }

        public static bool DeletarImagemProduto(string caminho)
        {
            string Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminho.TrimStart('/'));
            if (File.Exists(Caminho))
            {
                File.Delete(Caminho);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

//Continuar Módulo 15 Aula 31