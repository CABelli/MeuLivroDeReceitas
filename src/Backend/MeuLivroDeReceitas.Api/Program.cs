using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Infra.IoC;
using ResourcesApplication = MeuLivroDeReceitas.CrossCutting.Resources.Application;
using ResourcesCrossCutting = MeuLivroDeReceitas.CrossCutting.Resources.CrossCutting;
using ResourcesInfrastructure = MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var KeyVaultGlobalCultureLanguage = builder.Configuration.GetRequiredSection("KeyVault:KeyVaultGlobalCultureLanguage");
Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage.Value);
ResourcesApplication.Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage.Value);
ResourcesCrossCutting.Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage.Value);
ResourcesInfrastructure.Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage.Value);

// Add services to the container.

builder.Services.AddInfrastructureAPI(builder.Configuration);

//ativar autenticacao e validar o token
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

builder.Services.AddControllers();    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(MessageOrExceptionFilter)));

//builder.Services.AddAutoMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //context.Database.Migrate();

    var userManager = scope.ServiceProvider.GetRequiredService<ISeedUserRoleInitial>();
    userManager.SeedRoles();
    userManager.SeedUsers();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
