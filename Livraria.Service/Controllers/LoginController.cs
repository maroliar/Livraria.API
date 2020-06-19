using Livraria.Application.Contracts;
using Livraria.Application.Models.Usuario;
using Livraria.Utils.ResourceFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Livraria.Service.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioAppService appService;

        public LoginController(IUsuarioAppService appService)
        {
            this.appService = appService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public object Post([FromBody] UsuarioLoginModel model,
                            [FromServices] TokenConfiguration tokenConfiguration,
                            [FromServices] LoginConfiguration loginConfiguration)
        {
            if (ModelState.IsValid)
            {
                var usuario = appService.FindByLoginAndSenha(model);

                if (usuario != null)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(model.Login, "Login"),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, model.Login),
                            new Claim(ClaimTypes.Sid, usuario.IdUsuario.ToString())
                        }
                    );

                    //gerando o token
                    var creationDate = DateTime.Now;
                    var expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfiguration.Seconds);
                    var handler = new JwtSecurityTokenHandler();

                    var securityToken = handler.CreateToken(new
                    SecurityTokenDescriptor
                    {
                        Issuer = tokenConfiguration.Issuer,
                        Audience = tokenConfiguration.Audience,
                        SigningCredentials = loginConfiguration.SigningCredentials,
                        Subject = identity,
                        NotBefore = creationDate,
                        Expires = expirationDate
                    });

                    var token = handler.WriteToken(securityToken);
                    return new
                    {
                        authenticated = true,
                        created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
                else
                {
                    return BadRequest(
                    new
                    {
                        authenticated = false,
                        message = UsuarioResource.AcessoNegadoUsuarioInvalido
                    }
                    );
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
