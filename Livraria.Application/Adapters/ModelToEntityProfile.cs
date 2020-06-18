using AutoMapper;
using Livraria.Application.Models.Livro;
using Livraria.Application.Models.Usuario;
using Livraria.Domain.Entities;

namespace Livraria.Application.Adapters
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<UsuarioCadastroModel, Usuario>();

            CreateMap<LivroCadastroModel, Livro>();
            CreateMap<LivroEdicaoModel, Livro>();
        }
    }
}
