using Dinja.Extensions;
using Microsoft.OpenApi;
using System.Reflection;

const string corsRuleName = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer(); // Required for controllers
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TOTPGuard API",
        Version = "v1",
        Description = "API for TOTPGuard operations"
    });
});

// Allow Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsRuleName, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Register Dinja services
builder.Services.AddContainer(builder.Configuration, Assembly.GetExecutingAssembly());

// Building the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsRuleName);

app.UseAuthorization();

app.MapControllers();

app.Run();
