using BarberElite.Application.Barbeiros.DTOs;
using BarberElite.Application.Barbeiros.Interfaces;
using MediatR;

namespace BarberElite.Application.Barbeiros.Queries;

public sealed record GetBarbeiroByIdQuery(Guid Id) : IRequest<BarbeiroResponseDto>;

public sealed class GetBarbeiroByIdHandler : IRequestHandler<GetBarbeiroByIdQuery, BarbeiroResponseDto>
{
    private readonly IBarbeiroRepository _repo;

    public GetBarbeiroByIdHandler(IBarbeiroRepository repo) => _repo = repo;

    public async Task<BarbeiroResponseDto> Handle(GetBarbeiroByIdQuery request, CancellationToken ct)
    {
        var b = await _repo.GetByIdAsync(request.Id, ct);
        if (b is null)
            throw new KeyNotFoundException("Barbeiro não encontrado.");

        return new BarbeiroResponseDto(b.Id, b.UsuarioId, b.Nome, b.Ativo, b.CriadoEm);
    }
}
