using BarberElite.Application.Servicos.Interfaces;
using BarberElite.Domain.Entities;
using BarberElite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BarberElite.Application.Barbeiros.Interfaces;
using BarberElite.Infrastructure.Repositories;


namespace BarberElite.Infrastructure.Repositories;


public class BarbeiroRepository : IBarbeiroRepository
{
    private readonly BarberEliteDbContext _context;

    public BarbeiroRepository(BarberEliteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Barbeiro barbeiro, CancellationToken ct)
        => await _context.Barbeiros.AddAsync(barbeiro, ct);

    public async Task<Barbeiro?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Barbeiros.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<List<Barbeiro>> GetAllAsync(CancellationToken ct)
        => await _context.Barbeiros.ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);

    public Task<List<Barbeiro>> ListAtivosAsync(CancellationToken ct)
        => _context.Barbeiros.Where(x => x.Ativo).OrderBy(x => x.Nome).ToListAsync(ct);
}
