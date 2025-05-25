using System.Data;
using BackDoacaoDeAlimentos.Services;
using Microsoft.Data.SqlClient;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using Blazored.Toast;
using BackDoacaoDeAlimentos.Servicos;
using BackDoacaoDeAlimentos.Repositorios;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddScoped<IAlimentoRepositorio, AlimentoRepositorio>();
builder.Services.AddScoped<IAlimentoService, AlimentoService>();
builder.Services.AddScoped<IEntidadeService, EntidadeService>();
builder.Services.AddScoped<IEntidadeRepositorio, EntidadeRepositorio>();
builder.Services.AddScoped<IDoacaoService, DoacaoService>();
builder.Services.AddScoped<IDoacaoRepositorio, DoacaoRepositorio>();
builder.Services.AddScoped<INotificacaoRepositorio, NotificacaoRepositorio>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAlimentoDoacaoRepositorio, AlimentoDoacaoRepositorio>();
builder.Services.AddScoped<IAlimentoDoacaoService, AlimentoDoacaoService>();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7170") 
              .AllowAnyHeader()
              //.AllowAnyOrigin()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
