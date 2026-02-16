using BarberElite.Domain.Entities;

namespace BarberElite.Application.Servicos.Interfaces;

public interface IServicoRepository
{
    Task AddAsync(Servico servico, CancellationToken ct);
    Task<Servico?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Servico>> ListAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
