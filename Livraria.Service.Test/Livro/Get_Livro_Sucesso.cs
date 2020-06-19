using Livraria.Application.Contracts;
using Livraria.Application.Models.Livro;
using Livraria.Application.Models.Usuario;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Livraria.Service.Test.Livro
{
    [TestClass]
    public class Get_Livro_Sucesso : BaseBDDTestFixture
    {
        private Mock<ILivroAppService> appService;
        private Mock<IUsuarioAppService> usuarioAppService;
        private WebHostBuilder webHostBuilder;
        private HttpResponseMessage post;
        private UsuarioLoginModel loginModel;
        private string retorno;

        public override void Initialize()
        {
            appService = new Mock<ILivroAppService>();
            usuarioAppService = new Mock<IUsuarioAppService>();

            webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureTestServices(service => service.AddScoped(serviceProvider => appService.Object))
                          .ConfigureTestServices(service => service.AddScoped(serviceProvider => usuarioAppService.Object))
                          .UseStartup<Startup>();
        }

        public override void Given()
        {
            // Arrange

            loginModel = new UsuarioLoginModel
            {
                Login = "marcelo",
                Senha = "12345"
            };

            usuarioAppService.Setup(x => x.FindByLoginAndSenha(It.IsAny<UsuarioLoginModel>()))
                .Returns(new Domain.Entities.Usuario
                {
                    IdUsuario = 1,
                    Nome = "Usuario teste",
                    Senha = "senhacriptografada",
                    Login = "loginteste"
                });

            appService.Setup(x => x.FindAll()).Returns(new List<LivroConsultaModel>
            {
                new LivroConsultaModel
                {
                    Autor = "Autor 01",
                    DataPublicacao = DateTime.Now,
                    IdLivro = 1,
                    ImagemCapaUrl = "imgurl",
                    ISBN = 12345,
                    Nome = "Nome do Livro 01",
                    Preco = 10
                },
                new LivroConsultaModel
                {
                    Autor = "Autor 02",
                    DataPublicacao = DateTime.Now,
                    IdLivro = 1,
                    ImagemCapaUrl = "imgurl",
                    ISBN = 54321,
                    Nome = "Nome do Livro 02",
                    Preco = 20
                },
            });
        }

        public override void When()
        {
            var jsonContentLogin = JsonSerializer.Serialize(loginModel);
            var postContentLogin = new StringContent(jsonContentLogin, Encoding.UTF8, "application/json");

            using (var server = new TestServer(webHostBuilder))
            {

                using (var client = server.CreateClient())
                {
                    post = client.PostAsync("/api/login", postContentLogin).GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                var myJObject = JObject.Parse(retorno);
                var accessToken = myJObject.SelectToken("accessToken").Value<string>();

                using (var client = server.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    post = client.GetAsync("/api/livro").GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }

        [ThenAtrribute, TestCategory("EachBuild")]
        public void Quando_ObtenhoListaLivrosCadastrados_Entao_Sucesso()
        {
            // Assert
            Assert.IsNotNull(post);
            Assert.AreEqual(HttpStatusCode.OK, post.StatusCode);
            Assert.IsTrue(retorno.Contains("Autor 01"));
            Assert.IsTrue(retorno.Contains("Nome do Livro 01"));
            Assert.IsTrue(retorno.Contains("Autor 02"));
            Assert.IsTrue(retorno.Contains("Nome do Livro 02"));
        }
    }
}
