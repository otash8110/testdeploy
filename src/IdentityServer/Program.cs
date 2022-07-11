using IdentityServer;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Profile;
using Microsoft.EntityFrameworkCore;
using IdentityServer.UserValidator;
using Infrastructure.DataBaseContext;
using Application.Interfaces.IServices;
using Application.Services;
using Application.Interfaces.IDataBase;
using Domain.Entities;
using Infrastructure.Repositories;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args );

builder.Services.AddDbContext<DbAccessContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManager"));
});

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookies";
});

builder.Services.AddScoped<IGenericRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IGenericRepository<Role>, BaseRepository<Role>>();
builder.Services.AddScoped<IUsersService, UsersService>();

var assembly = typeof(Program).Assembly.GetName().Name;
var filePath = "locahlost.pfx";
var certificate = new X509Certificate2(filePath, "pass123");

builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddProfileService<ProfileService>()
    .AddResourceOwnerValidator<UserValidator>()
    .AddSigningCredential(certificate)
    .AddDeveloperSigningCredential();


var app = builder.Build();


app.UseIdentityServer();

app.Run();
