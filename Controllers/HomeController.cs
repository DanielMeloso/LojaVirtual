using LojaVirtual.Libraries.Email;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LojaVirtual.Database;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Models.ViewModels;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        //private LojaVirtualContext _banco;
        private IClienteRepository _repositoryCliente;
        private INewsletterRepository _repositoryNewsletter;
        private LoginCliente _loginCliente;
        private GerenciarEmail _gerenciarEmail;
        private IProdutoRepository _produtoRepository;
        public HomeController(IProdutoRepository produtoRepository, IClienteRepository repositoryCliente, INewsletterRepository repositoryNewsletter, LoginCliente loginCliente, GerenciarEmail gerenciarEmail)
        {
            _repositoryCliente = repositoryCliente;    
            _repositoryNewsletter = repositoryNewsletter;
            _loginCliente = loginCliente;
            _gerenciarEmail = gerenciarEmail;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                //_banco.NewsletterEmails.Add(newsletter);
                //_banco.SaveChanges();
                _repositoryNewsletter.Cadastrar(newsletter);
                TempData["MSG_S"] = "E-mail cadastrado! Agora você receberá promoções especiais em seu e-mail!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Categoria()
        {

            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            // essa forma de validar as informações requer mais etapas, a melhor forma é usar modelo do newsletter
            try
            {
                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var listaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                //em casos de erros na validação de informações, preenche a variavel listaMensagens
                bool isValid = Validator.TryValidateObject(contato, contexto, listaMensagens, true);

                if (isValid)
                {
                    _gerenciarEmail.EnviarContatoPorEmail(contato); //Envio de email
                    ViewData["MSG_SUCESSO"] = "Mensagem de contato enviada com sucesso!";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach ( var texto in listaMensagens)
                    {
                        sb.Append(texto.ErrorMessage + "<br />");
                    }

                    ViewData["MSG_ERROR"] = sb.ToString();
                    ViewData["CONTATO"] = contato; // quando existir erro, passar todas as informações para a View preencher os campos
                    // assim, evitará que o usuário perca o que ele já preencheu
                }  
            }
            catch (Exception e)
            {
                ViewData["MSG_ERROR"] = "Opps! Tivemos um erro, tente novamente mais tarde";

                //TODO - Implementar LOG
            }
            return View("Contato"); //nome da view que desejo retornar
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Cliente cliente)
        {
            // buscar informações no banco de dados para verificar se cliente já está cadastrado
            Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if(clienteDB != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário e senha inválidos";
                return View();
            }
        }

        [HttpGet]
        [ClienteAutorizacao] //filtro
        public IActionResult Painel()
        {
            return new ContentResult() {Content = "painel do cliente"};          
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                //_banco.Add(cliente);
                //_banco.SaveChanges();
                _repositoryCliente.Cadastrar(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                // TODO - Implementar redirecionamentos diferentes (painel, carrinho de compras, etc).
                return RedirectToAction(nameof(CadastroCliente));
            }
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}
