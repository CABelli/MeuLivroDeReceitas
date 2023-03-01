using MeuLivroDeReceitas.Infra.IoC;
using MeuLivroDeReceitas.Domain.Account;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructureAPI(builder.Configuration);

//ativar autenticacao e validar o token
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//app.Services.GetRequiredService<ISeedUserRoleInitial>().SeedRoles();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<ISeedUserRoleInitial>();
    userManager.SeedRoles();
    userManager.SeedUsers();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
