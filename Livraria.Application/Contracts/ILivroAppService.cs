using Livraria.Application.Models.Livro;
using Livraria.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Livraria.Application.Contracts
{
    public interface ILivroAppService : IDisposable
    {
        void Insert(LivroCadastroModel model);
        void Update(LivroEdicaoModel model);
        void Delete(int id);

        //Livro Find(Func<LivroConsultaModel, bool> predicate);
        LivroConsultaModel FindById(int id);
        ICollection<LivroConsultaModel> FindAll();
        //ICollection<Livro> FindAll(Func<LivroConsultaModel, bool> predicate);
        //int Count(Func<LivroConsultaModel, bool> predicate);
    }
}
