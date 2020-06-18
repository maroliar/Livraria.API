using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Livraria.Application.Models.Livro
{
    public class LivroConsultaModel
    {
        public virtual int IdLivro { get; set; }
        public int ISBN { get; set; }
        public string Autor { get; set; }

        [Display(Name = "Nome do Livro")]
        public string Nome { get; set; }

        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Display(Name = "Data de Publicação")]
        public DateTime DataPublicacao { get; set; }

        [Display(Name = "Capa do Livro")]
        public byte[] ImagemCapa { get; set; }
    }
}
