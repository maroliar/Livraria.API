using Livraria.Utils.ResourceFiles;
using System.ComponentModel.DataAnnotations;

namespace Livraria.Application.Models.Usuario
{
    public class UsuarioCadastroModel
    {
        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CampoNomeObrigatorio")]

        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CampoLoginObrigatorio")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CampoSenhaObrigatorio")]
        [StringLength(50, ErrorMessage = "A {0} precisa ter ao menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a Senha")]
        [Compare("Senha", ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "CamposSenhaConfirmacaoSenhaNaoConferem")]
        public string ConfirmacaoSenha { get; set; }
    }
}
