using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace LojaVirtual.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            /*
             * SMTP -> Servidor que irá enviar a mensagem.
             * https://www.google.com/settings/security/lesssecureapps -> habilitar app menos seguros
             */
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("daniel.crescer@gmail.com", "905906abc"); //incluir a senha no segundo parametro
            smtp.EnableSsl = true;

            string corpoMsg = string.Format("<h2> Contato - LojaVirtual Daniel </h2>" +
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
            mensagem.From = new MailAddress("daniel.crescer@gmail.com");
            mensagem.To.Add("daniel.crescer@gmail.com"); //coleção de emails, pode adicionar quantos for necessário.
            mensagem.Subject = "Contato - LojaVirtual Daniel - E-mail: " + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true; //habilitar o uso de HTML na mensagem de texto

            //Enviar mensagem via SMTP
            smtp.Send(mensagem);

        }
    }
}
