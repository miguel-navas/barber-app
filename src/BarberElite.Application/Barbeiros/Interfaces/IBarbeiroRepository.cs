using BarberElite.Domain.Entities;

namespace BarberElite.Application.Barbeiros.Interfaces;

public interface IBarbeiroRepository
{
    Task AddAsync(Barbeiro barbeiro, CancellationToken ct);
    Task<Barbeiro?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Barbeiro>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<List<Barbeiro>> ListAtivosAsync(CancellationToken ct);
}
