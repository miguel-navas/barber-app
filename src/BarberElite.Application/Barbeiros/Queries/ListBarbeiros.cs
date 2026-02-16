using BarberElite.Application.Barbeiros.DTOs;
using BarberElite.Application.Barbeiros.Interfaces;
using MediatR;

namespace BarberElite.Application.Barbeiros.Queries;

public sealed record ListBarbeirosQuery(bool ApenasAtivos = true) : IRequest<List<BarbeiroResponseDto>>;

public sealed class ListBarbeirosHandler : IRequestHandler<ListBarbeirosQuery, List<BarbeiroResponseDto>>
{
    private readonly IBarbeiroRepository _repo;

    public ListBarbeirosHandler(IBarbeiroRepository repo) => _repo = repo;

    public async Task<List<BarbeiroResponseDto>> Handle(ListBarbeirosQuery request, CancellationToken ct)
    {
        var list = request.ApenasAtivos
            ? await _repo.ListAtivosAsync(ct)
            : await _repo.GetAllAsync(ct);

        return list
            .Select(b => new BarbeiroResponseDto(b.Id, b.UsuarioId, b.Nome, b.Ativo, b.CriadoEm))
            .ToList();
    }

}
