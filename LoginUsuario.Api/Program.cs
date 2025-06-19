using FluentValidation;
using LoginUsuario.Application.UseCases.DoLogin;
using LoginUsuario.Application.UseCases.Register;
using LoginUsuario.Application.Validators;
using LoginUsuario.Comunication.Requests;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Infrastructure.Data;
using LoginUsuario.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<LoginUsuario.Application.UseCases.DoLogin.DoLoginUsuarioUseCase>();
builder.Services.AddScoped<LoginUsuario.Application.UseCases.Register.RegisterUsuarioUseCase>();

builder.Services.AddScoped<IValidator<RequestRegisterUsuarioJson>, RegisterUsuarioValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
