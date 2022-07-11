using Microsoft.EntityFrameworkCore;
using Infrastructure.DataBaseContext;
using Infrastructure.Repositories;
using Domain.Entities;
using Application.Services;
using Application.Interfaces.IServices;
using Application.Interfaces.IMappers;
using Application.Interfaces.IDataBase;
using Application.Interfaces.ITasksAdditional;
using Application.Services.MappersServices;
using Application.Services.ValidationsServices;
using Application.Services.AdditionalTasksServices;
using Application.DTO;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using API.Middlewares;
using Microsoft.OpenApi.Models;
using API.Services;
using API.IOptionsModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    //Delete ProblemDetails schems in swagger
    .ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; })
    //Register FluentValidation
    .AddFluentValidation(s =>
    {
        s.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "MyAPI";
        options.Authority = "https://localhost:3306";
        options.RequireHttpsMetadata = false;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:3306/connect/token"),
                Scopes = new Dictionary<string, string> { }
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
          new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
          new List<string>()
        }
    });

    //Use XML documentation to appear in swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//Add dependencies of the project
builder.Services.AddDbContext<DbAccessContext>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGenericRepository<ProjectTask>, BaseRepository<ProjectTask>>();
builder.Services.AddScoped<IGenericRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IGenericRepository<Role>, BaseRepository<Role>>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGenerateDescription, DescriptionGenerator>();
builder.Services.AddScoped<IProjectTaskMapper, ProjectTaskMapper>();
builder.Services.AddScoped(typeof(AbstractValidator<ProjectTaskDto>), typeof(ProjectTaskValidator));

builder.Services.Configure<Discovery>(builder.Configuration.GetSection("Discovery"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    System.Diagnostics.Debug.WriteLine(app.Configuration.GetConnectionString("TaskManager"));
    Console.WriteLine(app.Configuration.GetConnectionString("TaskManager"));
    var dbContext = scope.ServiceProvider.GetRequiredService<DbAccessContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("swagger_client");
        options.OAuthScopeSeparator(" ");
        options.OAuthClientSecret("secret");
    });

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();


