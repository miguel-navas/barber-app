using BarberElite.Application.Disponibilidades.DTOs;
using BarberElite.Application.Disponibilidades.Interfaces;
using MediatR;

namespace BarberElite.Application.Disponibilidades.Queries;

public sealed record ListDisponibilidadesByBarbeiroQuery(Guid BarbeiroId, bool ApenasAtivas = true)
    : IRequest<List<DisponibilidadeResponseDto>>;

public sealed class ListDisponibilidadesByBarbeiroHandler
    : IRequestHandler<ListDisponibilidadesByBarbeiroQuery, List<DisponibilidadeResponseDto>>
{
    private readonly IDisponibilidadeRepository _repo;

    public ListDisponibilidadesByBarbeiroHandler(IDisponibilidadeRepository repo) => _repo = repo;

    public async Task<List<DisponibilidadeResponseDto>> Handle(ListDisponibilidadesByBarbeiroQuery request, CancellationToken ct)
    {
        var list = await _repo.ListByBarbeiroAsync(request.BarbeiroId, ct);

        if (request.ApenasAtivas)
            list = list.Where(x => x.Ativo).ToList();

        return list
            .OrderBy(x => x.DiaSemana)
            .ThenBy(x => x.HoraInicio)
            .Select(d => new DisponibilidadeResponseDto(d.Id, d.BarbeiroId, d.DiaSemana, d.HoraInicio.ToString("HH:mm"), d.HoraFim.ToString("HH:mm"), d.Ativo, d.CriadoEm))
            .ToList();
    }
}
