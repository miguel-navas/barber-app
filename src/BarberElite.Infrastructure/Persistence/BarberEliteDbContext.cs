using BarberElite.Application.Common.Interfaces;
using BarberElite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberElite.Infrastructure.Persistence;

public class BarberEliteDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public BarberEliteDbContext(
        DbContextOptions<BarberEliteDbContext> options,
        ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Barbeiro> Barbeiros => Set<Barbeiro>();
    public DbSet<Servico> Servicos => Set<Servico>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Comissao> Comissoes => Set<Comissao>();
    public DbSet<DisponibilidadeHorario> Disponibilidades => Set<DisponibilidadeHorario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔹 Global Query Filter (Multi-tenant)
        modelBuilder.Entity<Usuario>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Usuario>()
            .HasIndex(x => new { x.TenantId, x.Email })
            .IsUnique();


        modelBuilder.Entity<Cliente>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Barbeiro>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Servico>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Agendamento>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Pagamento>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<Comissao>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        modelBuilder.Entity<DisponibilidadeHorario>()
            .HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

        // 🔹 Precision decimal (IMPORTANTE)
        modelBuilder.Entity<Servico>()
            .Property(x => x.Preco)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pagamento>()
            .Property(x => x.Valor)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Comissao>()
            .Property(x => x.Valor)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Comissao>()
            .Property(x => x.PercentualAplicado)
            .HasPrecision(5, 2);
    }
}
