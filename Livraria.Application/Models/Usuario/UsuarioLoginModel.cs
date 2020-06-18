using System.ComponentModel.DataAnnotations;
using Livraria.Utils.ResourceFiles;

namespace Livraria.Application.Models.Usuario
{
    public class UsuarioLoginModel
    {
        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CampoLoginObrigatorio")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CampoSenhaObrigatorio")]
        public string Senha { get; set; }
    }
}
