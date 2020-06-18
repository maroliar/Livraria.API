using System;

namespace Livraria.Domain.Entities
{
    public class Livro
    {
        public virtual int IdLivro { get; set; }
        public virtual int ISBN { get; set; }
        public virtual string Autor { get; set; }
        public virtual string Nome { get; set; }
        public virtual decimal Preco { get; set; }
        public virtual DateTime DataPublicacao { get; set; }
        public virtual byte[] ImagemCapa { get; set; }
        public virtual int IdUsuario { get; set; }

        //Navegabilidade
        public virtual Usuario Usuario { get; set; }

    }
}
