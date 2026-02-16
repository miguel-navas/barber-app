using BarberElite.Application.Barbeiros.Interfaces;
using BarberElite.Application.Disponibilidades.Interfaces;
using BarberElite.Application.Servicos.Interfaces;
using BarberElite.Infrastructure.Persistence;
using BarberElite.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberElite.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BarberEliteDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Repositories
        services.AddScoped<IServicoRepository, ServicoRepository>();
        services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
        services.AddScoped<IDisponibilidadeRepository, DisponibilidadeRepository>();

        return services;
    }
}
