using BarberElite.Application.Barbeiros.DTOs;
using BarberElite.Application.Barbeiros.Interfaces;
using BarberElite.Application.Common.Interfaces;
using BarberElite.Domain.Entities;
using MediatR;

namespace BarberElite.Application.Barbeiros.Commands;

public sealed record CreateBarbeiroCommand(Guid UsuarioId, string Nome) : IRequest<BarbeiroResponseDto>;

public sealed class CreateBarbeiroHandler : IRequestHandler<CreateBarbeiroCommand, BarbeiroResponseDto>
{
    private readonly IBarbeiroRepository _repo;
    private readonly ITenantProvider _tenant;

    public CreateBarbeiroHandler(IBarbeiroRepository repo, ITenantProvider tenant)
    {
        _repo = repo;
        _tenant = tenant;
    }

    public async Task<BarbeiroResponseDto> Handle(CreateBarbeiroCommand request, CancellationToken ct)
    {
        var tenantId = _tenant.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantId inválido. Envie o header X-Tenant-Id.");

        var barbeiro = Barbeiro.Criar(tenantId, request.UsuarioId, request.Nome);

        await _repo.AddAsync(barbeiro, ct);
        await _repo.SaveChangesAsync(ct);

        return new BarbeiroResponseDto(barbeiro.Id, barbeiro.UsuarioId, barbeiro.Nome, barbeiro.Ativo, barbeiro.CriadoEm);
    }
}
