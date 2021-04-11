using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Middleware
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private RequestDelegate _next;
        private IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            var Cabecalho = context.Request.Headers["x-requested-with"];
            bool AJAX = (Cabecalho == "XMLHttpRequest") ? true : false; // verificação se a requisição foi por meio de Ajax.

            if (HttpMethods.IsPost(context.Request.Method) && !(context.Request.Form.Files.Count == 1 && AJAX))
            {
                // bloqueia caso não tenha token ou token invalido
                await _antiforgery.ValidateRequestAsync(context);
            }

            // se não tiver nenhum problema na validação acima, continuará com a requisição normalmente.
            await _next(context);
        }
    }
}
