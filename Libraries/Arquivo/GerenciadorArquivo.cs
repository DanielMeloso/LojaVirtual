using LojaVirtual.Models;
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
            return Path.Combine("/uploads/temp", NomeArquivo).Replace("\\","/");
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

        public static List<Imagem> MoverImagensProduto(List<string> ListaCaminhoTemp, int _produtoId)
        {
            // Criar pasta para armazenar as imagens do produto
            var CaminhoDefinitivoPastaProduto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/imgProdutos", _produtoId.ToString());
            if (!Directory.Exists(CaminhoDefinitivoPastaProduto))
            {
                Directory.CreateDirectory(CaminhoDefinitivoPastaProduto); 
            }

            // Mover a imagem da pasta temporária para a pasta definitiva
            List<Imagem> ListaImagemDef = new List<Imagem>();
            foreach(var CaminhoImgTemp in ListaCaminhoTemp)
            {
                if (!string.IsNullOrEmpty(CaminhoImgTemp))
                {
                    var NomeArquivo = Path.GetFileName(CaminhoImgTemp);
                    var CaminhoAbsolutoTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", NomeArquivo);
                    var CaminhoAbsolutoDefinitivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/imgProdutos", _produtoId.ToString(), NomeArquivo);
    
                    if (File.Exists(CaminhoAbsolutoTemp))
                    {
                        // Mover arquivo
                        File.Copy(CaminhoAbsolutoTemp, CaminhoAbsolutoDefinitivo);

                        if (File.Exists(CaminhoAbsolutoDefinitivo))
                        {
                            File.Delete(CaminhoAbsolutoTemp);
                        }
                        ListaImagemDef.Add(
                            new Imagem()
                            {
                                Caminho = Path.Combine("/uploads/imgProdutos", _produtoId.ToString(), NomeArquivo).Replace("\\", "/"),
                                ProdutoId = _produtoId
                            });
                            
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            return ListaImagemDef;

        }
    }
}
