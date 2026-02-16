using BarberElite.Application.Common.Interfaces;
using BarberElite.Application.Servicos.DTOs;
using BarberElite.Application.Servicos.Interfaces;
using BarberElite.Domain.Entities;
using MediatR;

namespace BarberElite.Application.Servicos.Commands;

public sealed record CreateServicoCommand(ServicoCreateDto Dto) : IRequest<ServicoResponseDto>;

public sealed class CreateServicoHandler : IRequestHandler<CreateServicoCommand, ServicoResponseDto>
{
    private readonly ITenantProvider _tenantProvider;
    private readonly IServicoRepository _repo;

    public CreateServicoHandler(ITenantProvider tenantProvider, IServicoRepository repo)
    {
        _tenantProvider = tenantProvider;
        _repo = repo;
    }

    public async Task<ServicoResponseDto> Handle(CreateServicoCommand request, CancellationToken ct)
    {
        var tenantId = _tenantProvider.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantId inválido. Envie o header X-Tenant-Id.");

        var s = Servico.Criar(
            tenantId,
            request.Dto.Nome,
            request.Dto.DuracaoMinutos,
            request.Dto.Preco,
            request.Dto.Tipo,
            request.Dto.Descricao
        );

        await _repo.AddAsync(s, ct);
        await _repo.SaveChangesAsync(ct);

        return new ServicoResponseDto(s.Id, s.Nome, s.Descricao, s.DuracaoMinutos, s.Preco, s.Tipo, s.Ativo, s.CriadoEm);
    }
}
