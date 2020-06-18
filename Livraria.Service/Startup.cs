using AutoMapper;
using Livraria.Application.Contracts;
using Livraria.Application.Services;
using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Services;
using Livraria.Infra.Data.Context;
using Livraria.Infra.Data.Repositories;
using Livraria.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace Livraria.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Mapa de Dependencias de Repositorio

            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILivroRepository, LivroRepository>();

            services.AddTransient<DataContext>();
            services.AddTransient<ICryptorEngine, CryptorEngine>();

            services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();
            services.AddTransient<ILivroDomainService, LivroDomainService>();

            services.AddTransient<IUsuarioAppService, UsuarioAppService>();
            services.AddTransient<ILivroAppService, LivroAppService>();

            #endregion

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("livraria"))
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));

            services.AddTransient(db => new SqlConnection(Configuration.GetConnectionString("livraria")));
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // TODO
            //#region AppSettings
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //#endregion

            #region JWT

            var signingConfigurations = new LoginConfiguration();

            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfiguration"))
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(bearerOptions =>
                {
                    var paramsValidation = bearerOptions.TokenValidationParameters;

                    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                    paramsValidation.ValidAudience = tokenConfigurations.Audience;
                    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado 
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            #endregion

            #region AutoMapper

            services.AddAutoMapper(typeof(Startup));

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                services.AddControllers();

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Livraria",
                        Version = "v1",
                        Description = "Descrição da Livraria",
                        Contact = new OpenApiContact
                        {
                            Name = "Marcelo Oliveira",
                            Url = new Uri("https://github.com/maroliar"),
                            Email = "marcelouff@outlook.com"
                        }
                    });
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Livraria");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
