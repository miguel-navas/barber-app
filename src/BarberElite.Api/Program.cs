using Microsoft.OpenApi.Models;
using BarberElite.Api.Services;
using BarberElite.Application;
using BarberElite.Application.Common.Interfaces;
using BarberElite.Infrastructure;
using Microsoft.OpenApi.Models;
using BarberElite.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Controllers (se você vai usar Controllers)
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BarberElite API",
        Version = "v1"
    });

    c.AddSecurityDefinition("X-Tenant-Id", new OpenApiSecurityScheme
    {
        Description = "Tenant Id (GUID)",
        Name = "X-Tenant-Id",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-Tenant-Id"
                }
            },
            new string[] {}
        }
    });
});

// Tenant / Context
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, HeaderTenantProvider>();

// Clean Architecture registrations
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Middleware
builder.Services.AddTransient<TenantHeaderMiddleware>();

var app = builder.Build();

// Tenant header required (antes de mapear controllers)
app.UseMiddleware<TenantHeaderMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
