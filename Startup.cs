using LojaVirtual.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using LojaVirtual.Repositories;
using LojaVirtual.Repositories.Contracts;
using LojaVirtual.Libraries.Sessao;
using LojaVirtual.Libraries.Login;
using System.Net.Mail;
using System.Net;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Libraries.Middleware;

namespace LojaVirtual
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            
            /*
             * Padrão Repository
             */
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IImagemRepository, ImagemRepository>();

            /*
             * SMTP -> Servidor que irá enviar a mensagem.
             * https://www.google.com/settings/security/lesssecureapps -> habilitar app menos seguros
             */
            services.AddScoped<SmtpClient>(options => {
                SmtpClient smtp = new SmtpClient()
                {
                    //Host = Configuration.GetValue<string>("Email:ServidorSMTP"),
                    //Port = Configuration.GetValue<int>("Email:ServidorPorta"),
                    //UseDefaultCredentials = false,
                    //Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:EmailEnvio"),Configuration.GetValue<string>("Email:SenhaEmail")),
                    //EnableSsl = true

                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("daniel.crescer@gmail.com", "905906abc"),
                    EnableSsl = true
                };
                return smtp;
            });
            services.AddScoped<GerenciarEmail>();


            // Session -> Configuração
            services.AddMemoryCache(); // Guardar os dados na memória
            services.AddSession(options =>
            {

            });
            services.AddHttpContextAccessor();
            services.AddScoped<Sessao>();
            services.AddScoped<LoginCliente>();
            services.AddScoped<LoginColaborador>();

            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False; Database=LojaVirtual";
            services.AddDbContext<LojaVirtualContext>(options => options.UseSqlServer(connection)); 
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseDefaultFiles(); //usar arquivo padrão
            app.UseStaticFiles();
            app.UseSession();
            // classe criada para bloquear requisições de fora da aplicação.
            // com essa classe, bloqueia tanto requisições POST quanto GET
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });*/
        }
    }
}
