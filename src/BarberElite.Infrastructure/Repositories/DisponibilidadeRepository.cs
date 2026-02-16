using BarberElite.Application.Disponibilidades.Interfaces;
using BarberElite.Domain.Entities;
using BarberElite.Domain.Enums;
using BarberElite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BarberElite.Infrastructure.Repositories;

public sealed class DisponibilidadeRepository : IDisponibilidadeRepository
{
    private readonly BarberEliteDbContext _db;

    public DisponibilidadeRepository(BarberEliteDbContext db) => _db = db;

    public Task AddAsync(DisponibilidadeHorario disponibilidade, CancellationToken ct)
        => _db.Disponibilidades.AddAsync(disponibilidade, ct).AsTask();

    public Task<DisponibilidadeHorario?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Disponibilidades.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<DisponibilidadeHorario>> ListByBarbeiroAsync(Guid barbeiroId, CancellationToken ct)
        => _db.Disponibilidades
            .Where(x => x.BarbeiroId == barbeiroId)
            .ToListAsync(ct);

    public Task<List<DisponibilidadeHorario>> ListAtivasByBarbeiroDiaAsync(Guid barbeiroId, DiaSemana diaSemana, CancellationToken ct)
        => _db.Disponibilidades
            .Where(x => x.BarbeiroId == barbeiroId && x.DiaSemana == diaSemana && x.Ativo)
            .ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
