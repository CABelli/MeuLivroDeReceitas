using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Infrastructure.Context;
using MeuLivroDeReceitas.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infra.IoC
{
    public static  class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
            IConfiguration configuration)
        {            
            services.AddDbContext<ApplicationDbContext>(opions =>
            opions.UseSqlServer(configuration.GetConnectionString("DefaultConnection"
            ), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Account/Login");


            services.AddScoped<IAuthenticate, AuthenticateService>();
            //services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

            //services.AddAutoMapper(typeof(DomainToDTOMappingProfile));            

            var myHandlers = AppDomain.CurrentDomain.Load("MeuLivroDeReceitas.Application");
            services.AddMediatR(myHandlers);

            return services;
        }
    }
}
