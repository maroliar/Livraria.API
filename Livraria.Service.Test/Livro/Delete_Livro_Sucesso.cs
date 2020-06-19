using Livraria.Application.Contracts;
using Livraria.Application.Models.Livro;
using Livraria.Application.Models.Usuario;
using Livraria.Domain.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Livraria.Service.Test.Livro
{
    [TestClass]
    public class Delete_Livro_Sucesso: BaseBDDTestFixture
    {
        private Mock<IUsuarioAppService> usuarioAppService;
        private Mock<ILivroAppService> livroAppService;
        private WebHostBuilder webHostBuilder;
        private HttpResponseMessage post;
        private UsuarioLoginModel loginModel;
        private string retorno;

        public override void Initialize()
        {
            usuarioAppService = new Mock<IUsuarioAppService>();
            livroAppService = new Mock<ILivroAppService>();

            webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureTestServices(service => service.AddScoped(serviceProvider => usuarioAppService.Object))
                          .ConfigureTestServices(service => service.AddScoped(serviceProvider => livroAppService.Object))
                          .UseStartup<Startup>();
        }

        public override void Given()
        {
            // Arrange

            loginModel = new UsuarioLoginModel
            {
                Login = "marcelo",
                Senha = "a3eilm"
            };

            usuarioAppService.Setup(x => x.FindByLoginAndSenha(It.IsAny<UsuarioLoginModel>()))
                .Returns(new Domain.Entities.Usuario
                {
                    IdUsuario = 1,
                    Nome = "Usuario teste",
                    Senha = "senhacriptografada",
                    Login = "loginteste"
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

                    post = client.DeleteAsync("/api/livro?id=1").GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }

        [ThenAtrribute, TestCategory("EachBuild")]
        public void Quando_DeletoUmLivroCadastrado_InformandoSeuId_Entao_Sucesso()
        {
            // Assert
            Assert.IsNotNull(post);
            Assert.AreEqual(HttpStatusCode.OK, post.StatusCode);
            Assert.IsTrue(retorno.Contains("Livro excluido com sucesso!"));
        }
    }
}
