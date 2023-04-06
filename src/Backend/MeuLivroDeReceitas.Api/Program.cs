using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Infra.IoC;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using ResourcesApplication = MeuLivroDeReceitas.CrossCutting.Resources.Application;

var builder = WebApplication.CreateBuilder(args);

var KeyVaultGlobalCultureLanguage = "pt"; /// en pt(Default)
Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage);
ResourcesApplication.Resource.Culture = new System.Globalization.CultureInfo(KeyVaultGlobalCultureLanguage);

// Add services to the container.

builder.Services.AddInfrastructureAPI(builder.Configuration);

//ativar autenticacao e validar o token
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

builder.Services.AddControllers();    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)));

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

//using (var scope = app.Services.CreateScope())
//{
//    //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    //context.Database.Migrate();

//    var userManager = scope.ServiceProvider.GetRequiredService<ISeedUserRoleInitial>();
//    userManager.SeedRoles();
//    userManager.SeedUsers();
//}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
