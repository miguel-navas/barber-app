using BarberElite.Application.Servicos.Interfaces;
using BarberElite.Domain.Entities;
using BarberElite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BarberElite.Infrastructure.Repositories;

public sealed class ServicoRepository : IServicoRepository
{
    private readonly BarberEliteDbContext _db;

    public ServicoRepository(BarberEliteDbContext db) => _db = db;

    public Task AddAsync(Servico servico, CancellationToken ct)
        => _db.Servicos.AddAsync(servico, ct).AsTask();

    public Task<Servico?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Servicos.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<Servico>> ListAsync(CancellationToken ct)
        => _db.Servicos
            .OrderByDescending(x => x.CriadoEm)
            .ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
