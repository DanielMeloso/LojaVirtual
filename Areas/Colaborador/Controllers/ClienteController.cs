using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models;
using LojaVirtual.Models.Constants;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        
        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList<Cliente> clientes = _clienteRepository.ObterTodosClientes(pagina, pesquisa);
            return View(clientes);
        }

        //TODO implementar validação
        //[ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
            Cliente cliente = _clienteRepository.ObterCliente(id);

            if(cliente.Situacao == SituacaoConstante.Ativo)
            {
                cliente.Situacao = SituacaoConstante.Inativo;
            }
            else
            {
                cliente.Situacao = SituacaoConstante.Ativo;
            }

            _clienteRepository.Atualizar(cliente);
            TempData["MSG_S"] = Mensagem.MSG_S001;
            return RedirectToAction(nameof(Index));

        }
    }
}
