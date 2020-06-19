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
    public class Post_Livro_Cenario2_InternalServerError : BaseBDDTestFixture
    {
        private Mock<IUsuarioAppService> usuarioAppService;
        private Mock<ILivroDomainService> livroDomainService;
        private WebHostBuilder webHostBuilder;
        private HttpResponseMessage post;
        private LivroCadastroModel model;
        private UsuarioLoginModel loginModel;
        private string retorno;

        public override void Initialize()
        {
            usuarioAppService = new Mock<IUsuarioAppService>();
            livroDomainService = new Mock<ILivroDomainService>();

            webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureTestServices(service => service.AddScoped(serviceProvider => usuarioAppService.Object))
                          .ConfigureTestServices(service => service.AddScoped(serviceProvider => livroDomainService.Object))
                          .UseStartup<Startup>();
        }

        public override void Given()
        {
            // Arrange

            model = new LivroCadastroModel
            {
                Autor = "Autor Teste",
                DataPublicacao = DateTime.Now,
                ImagemCapaUrl = "imageurl/imagename.png",
                ISBN = 12345,
                Nome = "Nome Livro Teste",
                Preco = 10
            };

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

            livroDomainService.Setup(x => x.Insert(It.IsAny<Domain.Entities.Livro>())).Throws(new Exception("ISBN já está cadastrado."));
        }

        public override void When()
        {
            var jsonContentLogin = JsonSerializer.Serialize(loginModel);
            var postContentLogin = new StringContent(jsonContentLogin, Encoding.UTF8, "application/json");


            var jsonContentLivro = JsonSerializer.Serialize(model);
            var postContentLivro = new StringContent(jsonContentLivro, Encoding.UTF8, "application/json");

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

                    post = client.PostAsync("/api/livro", postContentLivro).GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }

        [ThenAtrribute, TestCategory("EachBuild")]
        public void Quando_CadastroNovoLivro_InformandoCodigoISBNJaCadastrado_Entao_Erro()
        {
            // Assert
            Assert.IsNotNull(post);
            Assert.AreEqual(HttpStatusCode.InternalServerError, post.StatusCode);
            Assert.IsTrue(retorno.Contains("ISBN já está cadastrado."));
        }
    }
}
