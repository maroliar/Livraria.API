using AutoMapper;
using Livraria.Application.Contracts;
using Livraria.Application.Models.Livro;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Entities;
using Livraria.Utils.ResourceFiles;
using System;
using System.Collections.Generic;

namespace Livraria.Application.Services
{
    public class LivroAppService : ILivroAppService
    {
        private readonly ILivroDomainService domain;
        private readonly IMapper mapper;

        public LivroAppService(ILivroDomainService domain, IMapper mapper)
        {
            this.domain = domain;
            this.mapper = mapper;
        }

        public void Insert(LivroCadastroModel model)
        {
            var livro = mapper.Map<Livro>(model);
            domain.Insert(livro);
        }

        public void Update(LivroEdicaoModel model)
        {
            var livro = mapper.Map<Livro>(model);
            domain.Update(livro);
        }

        public void Delete(int id)
        {
            var livro = domain.FindById(id);
            domain.Delete(livro);           
        }

        //public Livro Find(Func<Livro, bool> predicate)
        //{
        //    return domain.Find(predicate);
        //}

        public LivroConsultaModel FindById(int id)
        {
            var model = mapper.Map<LivroConsultaModel>(domain.FindById(id));
            return model;
        }
        public ICollection<LivroConsultaModel> FindAll()
        {
            var modelCollection = mapper.Map<ICollection<LivroConsultaModel>>(domain.FindAll());
            return modelCollection;
        }

        //public ICollection<Livro> FindAll(Func<Livro, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //public int Count(Func<Livro, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
