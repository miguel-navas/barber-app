using BarberElite.Domain.Entities;
using BarberElite.Domain.Enums;

namespace BarberElite.Application.Disponibilidades.Interfaces;

public interface IDisponibilidadeRepository
{
    Task AddAsync(DisponibilidadeHorario disponibilidade, CancellationToken ct);
    Task<DisponibilidadeHorario?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<DisponibilidadeHorario>> ListByBarbeiroAsync(Guid barbeiroId, CancellationToken ct);
    Task<List<DisponibilidadeHorario>> ListAtivasByBarbeiroDiaAsync(Guid barbeiroId, DiaSemana diaSemana, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}
