using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LojaVirtual.Libraries.Login
{
    public class LoginCliente
    {
        private string Key = "Login.Cliente";
        private Sessao.Sessao _sessao;
        public LoginCliente(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }
        public void Login(Cliente cliente)
        {
            // Armazenar na sessão
            // Serializar - Usando Newtonsoft.json
            string clienteJsonString = JsonConvert.SerializeObject(cliente); //converte objeto em string
            _sessao.Cadastrar(Key, clienteJsonString);
        }
        public Cliente GetCliente()
        {
            // Deserializar - Usando Newtonsoft.json
            if (_sessao.Existe(Key)){
                string clienteJsonString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJsonString); // converte string em objeto
            }
            else
            {
                return null;
            }
            
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
