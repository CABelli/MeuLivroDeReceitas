﻿using MediatR;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Application.Services;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Domain.EntityGeneric;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Domain.InterfacesGeneric;
using MeuLivroDeReceitas.Domain.InterfacesRepository;
using MeuLivroDeReceitas.Infrastructure.Context;
using MeuLivroDeReceitas.Infrastructure.Identity;
using MeuLivroDeReceitas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(opions =>
            opions.UseSqlServer(configuration.GetConnectionString("DefaultConnection"
            ), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<IAuthenticate, AuthenticateService>();
            services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();

            AdicionarTokenJWT(services, configuration);

            services.AddScoped<IRecipeService, RecipeService>();

            //services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            var myHandlers = AppDomain.CurrentDomain.Load("MeuLivroDeReceitas.Application");

            services.AddMediatR(myHandlers);

            return services;
        }

        private static void AdicionarTokenJWT(IServiceCollection services, IConfiguration configuration)
        {
            var sectionKey = configuration.GetRequiredSection("Jwt:SecretKey");
            var sectioLifeTime = configuration.GetRequiredSection("Jwt:SectioLifeTime").ToString();
            services.AddScoped(option => new TokenService(sectioLifeTime.ToDouble(), sectionKey.Value));
        }

    }
}
