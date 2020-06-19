using Livraria.Application.Contracts;
using Livraria.Application.Services;
using Livraria.Domain.Contracts.Identity;
using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Services;
using Livraria.Infra.Data.Context;
using Livraria.Infra.Data.Identity;
using Livraria.Infra.Data.Repositories;
using Livraria.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Livraria.Service.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) 
                throw new ArgumentNullException(nameof(services));

            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILivroRepository, LivroRepository>();

            services.AddTransient<DataContext>();
            services.AddTransient<ICryptorEngine, CryptorEngine>();

            services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();
            services.AddTransient<ILivroDomainService, LivroDomainService>();

            services.AddTransient<IUsuarioAppService, UsuarioAppService>();
            services.AddTransient<ILivroAppService, LivroAppService>();

            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
