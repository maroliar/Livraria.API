using AutoMapper;
using Livraria.Application.Models.Livro;
using Livraria.Application.Models.Usuario;
using Livraria.Domain.Entities;
using System;

namespace Livraria.Application.Adapters
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<UsuarioCadastroModel, Usuario>();

            CreateMap<LivroCadastroModel, Livro>();
                //.AfterMap((src, dest) => dest.ImagemCapa = Convert.FromBase64String(src.ImagemCapa));

            CreateMap<LivroEdicaoModel, Livro>();
        }
    }
}
