using LojaVirtual.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Login
{
    public class LoginColaborador
    {
        private string Key = "Login.Colaborador";
        private Sessao.Sessao _sessao;
        public LoginColaborador(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }
        public void Login(Colaborador colaborador)
        {
            // Armazenar na sessão
            // Serializar - Usando Newtonsoft.json
            string colaboradorJsonString = JsonConvert.SerializeObject(colaborador); //converte objeto em string
            _sessao.Cadastrar(Key, colaboradorJsonString);
        }
        public Colaborador GetColaborador()
        {
            // Deserializar - Usando Newtonsoft.json
            if (_sessao.Existe(Key))
            {
                string colaboradorJsonString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJsonString); // converte string em objeto
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
