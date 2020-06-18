using System.Collections.Generic;

namespace Livraria.Domain.Entities
{
    public class Usuario
    {
        public virtual int IdUsuario { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }

        // Navegabilidade
        public virtual ICollection<Livro> Livros { get; set; }
    }
}
