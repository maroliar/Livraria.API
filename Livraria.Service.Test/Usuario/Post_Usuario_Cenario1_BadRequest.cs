using Livraria.Application.Contracts;
using Livraria.Application.Models.Usuario;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Livraria.Service.Test.Usuario
{
    [TestClass]
    public class Post_Usuario_Cenario1_BadRequest : BaseBDDTestFixture
    {
        private Mock<IUsuarioAppService> appService;
        private WebHostBuilder webHostBuilder;
        private HttpResponseMessage post;
        private UsuarioCadastroModel model;
        private string retorno;

        public override void Initialize()
        {
            appService = new Mock<IUsuarioAppService>();

            webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureTestServices(service => service.AddScoped(serviceProvider => appService.Object)).UseStartup<Startup>();
        }

        public override void Given()
        {
            // Arrange
            model = new UsuarioCadastroModel
            {
                Nome = "",
                Login = "",
                Senha = "",
                ConfirmacaoSenha = ""
            };
        }

        public override void When()
        { 
            var jsonContent = JsonSerializer.Serialize(model);
            var postContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var server = new TestServer(webHostBuilder))
            {
                using (var client = server.CreateClient())
                {
                    post = client.PostAsync("/api/usuario", postContent).GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }

        [ThenAtrribute, TestCategory("EachBuild")]
        public void Quando_CadastroNovoUsuario_SemPreencherCamposCorretamente_Entao_Erro()
        {
            // Assert
            Assert.IsNotNull(post);
            Assert.AreEqual(HttpStatusCode.BadRequest, post.StatusCode);
            Assert.IsTrue(retorno.Contains("Campo Nome é obrigatório."));
            Assert.IsTrue(retorno.Contains("Campo Login é obrigatório."));
            Assert.IsTrue(retorno.Contains("Campo Senha é obrigatório."));
            Assert.IsTrue(retorno.Contains("A Senha precisa ter ao menos 6 caracteres."));
        }
    }
}
