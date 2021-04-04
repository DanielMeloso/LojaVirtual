using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace LojaVirtual.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;
        public GerenciarEmail(SmtpClient smtp, IConfiguration configuration)
        {
            _smtp = smtp;
            _configuration = configuration;
        }
        public void EnviarContatoPorEmail(Contato contato)
        {
            /*
             * SMTP -> Servidor que irá enviar a mensagem.
             * https://www.google.com/settings/security/lesssecureapps -> habilitar app menos seguros
             */

            string corpoMsg = string.Format("<h2> Contato - Loja Virtual Daniel </h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                "<br />E-mail enviado automaticamente do site LojaVirtual Daniel.",
                contato.Nome,
                contato.Email,
                contato.Texto
                ); 

            /*
             * mailMessage -> Construir a mensagem
             */

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email.EmailEnvio"));
            mensagem.To.Add("daniel.crescer@gmail.com"); //coleção de emails, pode adicionar quantos for necessário.
            mensagem.Subject = "Contato - LojaVirtual Daniel - E-mail: " + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true; //habilitar o uso de HTML na mensagem de texto

            //Enviar mensagem via SMTP
            _smtp.Send(mensagem);

        }

        public void EnviarSenhaColaboradorEmail(Colaborador colaborador)
        {
            string corpoMsg = string.Format("<h2> COLABORADOR - Senha de Acesso - Loja Virtual Daniel </h2>" +
                "Bem vindo(a) <b> {0} </b> <br />" +
                "Para realizar o acesso no sistema, utilize:" +
                "Email: <b> {1} </b> <br />" +
                "Senha: <b> {2} </b> <br />" +
                "<br />E-mail enviado automaticamente do site LojaVirtual Daniel.",
                colaborador.Nome,
                colaborador.Email,
                colaborador.Senha
                );

            /*
             * mailMessage -> Construir a mensagem
             */

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress("daniel.crescer@gmail.com");
            mensagem.To.Add(colaborador.Email); //coleção de emails, pode adicionar quantos for necessário.
            mensagem.Subject = "COLABORADOR - LojaVirtual Daniel - Acesso: " + colaborador.Nome;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true; //habilitar o uso de HTML na mensagem de texto

            //Enviar mensagem via SMTP
            _smtp.Send(mensagem);
        }
    }
}
