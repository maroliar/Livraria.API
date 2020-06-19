using Livraria.Application.Contracts;
using Livraria.Application.Models.Usuario;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Livraria.Service.Test.Login
{
    [TestClass]
    public class Post_Login_Sucesso : BaseBDDTestFixture
    {
        private Mock<IUsuarioAppService> appService;
        private WebHostBuilder webHostBuilder;
        private HttpResponseMessage post;
        private UsuarioLoginModel model;
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

            model = new UsuarioLoginModel
            {
                Login = "loginteste",
                Senha = "senhateste",
            };

            appService.Setup(x => x.FindByLoginAndSenha(It.IsAny<UsuarioLoginModel>()))
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
            var jsonContent = JsonSerializer.Serialize(model);
            var postContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var server = new TestServer(webHostBuilder))
            {
                using (var client = server.CreateClient())
                {
                    post = client.PostAsync("/api/login", postContent).GetAwaiter().GetResult();
                    retorno = post.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }

        [ThenAtrribute, TestCategory("EachBuild")]
        public void Quando_EfetuoLoginUsuario_ComDadosPreenchidosCorretamente_Entao_Sucesso()
        {
            var myJObject = JObject.Parse(retorno);
            
            var authenticated = myJObject.SelectToken("authenticated").Value<bool>();
            var created  = myJObject.SelectToken("created").Value<DateTime>();
            var expiration = myJObject.SelectToken("expiration").Value<DateTime>();
            var accessToken = myJObject.SelectToken("accessToken").Value<string>();
            var message = myJObject.SelectToken("message").Value<string>();

            // Assert
            Assert.IsNotNull(post);
            Assert.AreEqual(HttpStatusCode.OK, post.StatusCode);
            Assert.IsTrue(authenticated);
            Assert.AreEqual(typeof(DateTime), created.GetType());
            Assert.AreEqual(typeof(DateTime), expiration.GetType());
            Assert.IsTrue(expiration > created);
            Assert.IsNotNull(accessToken);
            Assert.AreEqual("OK", message);

        }
    }
}
