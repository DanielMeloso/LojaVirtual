using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        //const int RegistroPorPagina = 10; // quantidade de registros que será consultado no banco de dados a cada requisição
        private IConfiguration _conf;
        private LojaVirtualContext _banco;
        public ColaboradorRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            // só estou passando o Iconfiguration para usar a constante REGISTRO POR PAGINA que esta configurada em appsettings
            // caso contrátio, não há essa necessidade.
            _banco = banco;
            _conf = configuration;
        }
        public void Atualizar(Colaborador colaborador)
        {
            // Atualiza o Nome, Tipo e Email
            _banco.Update(colaborador);
            _banco.Entry(colaborador).Property(a => a.Senha).IsModified = false; // altera todos os campos, menos a senha
            _banco.SaveChanges();
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            // Atualiza a senha
            _banco.Update(colaborador);
            // atualiza somente a senha
            _banco.Entry(colaborador).Property(a => a.Nome).IsModified = false; 
            _banco.Entry(colaborador).Property(a => a.Email).IsModified = false; 
            _banco.Entry(colaborador).Property(a => a.Tipo).IsModified = false; 
            _banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            _banco.Add(colaborador);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Colaborador colaborador = ObterColaborador(Id);
            _banco.Remove(colaborador);
            _banco.SaveChanges();
        }

        public Colaborador Login(string Email, string Senha)
        {
            Colaborador colaborador = _banco.Colaboradores.Where(m => m.Email == Email && m.Senha == Senha).FirstOrDefault();
            return colaborador;
        }

        public Colaborador ObterColaborador(int Id)
        {
            return _banco.Colaboradores.Find(Id);
        }

        public List<Colaborador> ObterColaboradorPorEmail(string Email)
        {
            return _banco.Colaboradores.Where(a => a.Email == Email).AsNoTracking().ToList();
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            return _banco.Colaboradores
                .Where(a => a.Tipo != "G") // não exibir os gerentes na listagem
                .ToList();
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return _banco.Colaboradores
                .Where(a => a.Tipo != "G") // não exibir os gerentes na listagem
                .ToPagedList<Colaborador>(NumeroPagina, _conf.GetValue<int>("RegistroPorPagina"));
        }
    }
}
