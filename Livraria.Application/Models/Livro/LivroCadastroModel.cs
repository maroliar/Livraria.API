using Livraria.Utils.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Livraria.Application.Models.Livro
{
    public class LivroCadastroModel
    {
        [Required(ErrorMessageResourceType = typeof(LivroResource), ErrorMessageResourceName = "CampoISBNObrigatorio")]
        public int ISBN { get; set; }

        [Required(ErrorMessageResourceType = typeof(LivroResource), ErrorMessageResourceName = "CampoAutorObrigatorio")]
        public string Autor { get; set; }

        [Required(ErrorMessageResourceType = typeof(LivroResource), ErrorMessageResourceName = "CampoNomeLivroObrigatorio")]
        [Display(Name = "Nome do Livro")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(LivroResource), ErrorMessageResourceName = "CampoPrecoObrigatorio")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Required(ErrorMessageResourceType = typeof(LivroResource), ErrorMessageResourceName = "CampoDataPublicacaoObrigatorio")]
        [Display(Name = "Data de Publicação")]
        public DateTime DataPublicacao { get; set; }

        [Display(Name = "Upload da Capa do Livro")]
        public byte[] ImagemCapa { get; set; }
    }
}
