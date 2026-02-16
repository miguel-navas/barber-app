using BarberElite.Application.Barbeiros.DTOs;
using BarberElite.Application.Barbeiros.Interfaces;
using MediatR;

namespace BarberElite.Application.Barbeiros.Commands;

public sealed record UpdateBarbeiroCommand(Guid Id, BarbeiroUpdateDto Dto) : IRequest<BarbeiroResponseDto>;

public sealed class UpdateBarbeiroHandler : IRequestHandler<UpdateBarbeiroCommand, BarbeiroResponseDto>
{
    private readonly IBarbeiroRepository _repo;

    public UpdateBarbeiroHandler(IBarbeiroRepository repo) => _repo = repo;

    public async Task<BarbeiroResponseDto> Handle(UpdateBarbeiroCommand request, CancellationToken ct)
    {
        var b = await _repo.GetByIdAsync(request.Id, ct);
        if (b is null)
            throw new KeyNotFoundException("Barbeiro não encontrado.");

        b.Atualizar(request.Dto.Nome, request.Dto.Ativo);

        await _repo.SaveChangesAsync(ct);

        return new BarbeiroResponseDto(b.Id, b.UsuarioId, b.Nome, b.Ativo, b.CriadoEm);
    }
}
