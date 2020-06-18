using AutoMapper;
using Livraria.Application.Models.Livro;
using Livraria.Domain.Entities;

namespace Livraria.Application.Adapters
{
    public class EntityToModelProfile : Profile
    {
        public EntityToModelProfile()
        {
            CreateMap<Livro, LivroConsultaModel>();
        }
    }
}
