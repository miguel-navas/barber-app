using BarberElite.Application.Servicos.DTOs;
using BarberElite.Application.Servicos.Interfaces;
using MediatR;

namespace BarberElite.Application.Servicos.Queries;

public sealed record ListServicosQuery() : IRequest<List<ServicoResponseDto>>;

public sealed class ListServicosHandler : IRequestHandler<ListServicosQuery, List<ServicoResponseDto>>
{
    private readonly IServicoRepository _repo;

    public ListServicosHandler(IServicoRepository repo) => _repo = repo;

    public async Task<List<ServicoResponseDto>> Handle(ListServicosQuery request, CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);
        return list
            .Select(s => new ServicoResponseDto(
                s.Id,
                s.Nome,
                s.Descricao,
                s.DuracaoMinutos,
                s.Preco,
                s.Tipo,
                s.Ativo,
                s.CriadoEm
            ))
            .ToList();
    }
}
