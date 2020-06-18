using Livraria.Application.Contracts;
using Livraria.Application.Models.Usuario;
using Livraria.Utils.ResourceFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Livraria.Service.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAppService appService;

        public UsuarioController(IUsuarioAppService appService)
        {
            this.appService = appService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] UsuarioCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appService.Insert(model);

                    return Ok(UsuarioResource.UsuarioCadastradoSucesso);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
