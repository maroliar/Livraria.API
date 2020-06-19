using AutoMapper;
using Livraria.Application.Adapters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Livraria.Service.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) 
                throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(EntityToModelProfile), typeof(ModelToEntityProfile));
        }
    }
}
