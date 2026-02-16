using BarberElite.Application.Common.Interfaces;
using BarberElite.Application.Disponibilidades.DTOs;
using BarberElite.Application.Disponibilidades.Interfaces;
using BarberElite.Domain.Entities;
using MediatR;

namespace BarberElite.Application.Disponibilidades.Commands;

public sealed record CreateDisponibilidadeCommand(DisponibilidadeCreateDto Dto) : IRequest<DisponibilidadeResponseDto>;

public sealed class CreateDisponibilidadeHandler : IRequestHandler<CreateDisponibilidadeCommand, DisponibilidadeResponseDto>
{
    private readonly ITenantProvider _tenant;
    private readonly IDisponibilidadeRepository _repo;

    public CreateDisponibilidadeHandler(ITenantProvider tenant, IDisponibilidadeRepository repo)
    {
        _tenant = tenant;
        _repo = repo;
    }

    public async Task<DisponibilidadeResponseDto> Handle(CreateDisponibilidadeCommand request, CancellationToken ct)
    {
        var tenantId = _tenant.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantId inválido. Envie o header X-Tenant-Id.");

        if (!TimeOnly.TryParse(request.Dto.HoraInicio, out var inicio))
            throw new ArgumentException("HoraInicio inválida. Use formato HH:mm.");
        if (!TimeOnly.TryParse(request.Dto.HoraFim, out var fim))
            throw new ArgumentException("HoraFim inválida. Use formato HH:mm.");

        // ✅ regra: não pode sobrepor horários já ativos no mesmo dia
        var existentes = await _repo.ListAtivasByBarbeiroDiaAsync(request.Dto.BarbeiroId, request.Dto.DiaSemana, ct);
        foreach (var e in existentes)
        {
            var sobrepoe = inicio < e.HoraFim && fim > e.HoraInicio;
            if (sobrepoe)
                throw new InvalidOperationException("Conflito: horário sobrepõe outra disponibilidade ativa do barbeiro.");
        }

        var disp = DisponibilidadeHorario.Criar(tenantId, request.Dto.BarbeiroId, request.Dto.DiaSemana, inicio, fim);

        await _repo.AddAsync(disp, ct);
        await _repo.SaveChangesAsync(ct);

        return new DisponibilidadeResponseDto(disp.Id, disp.BarbeiroId, disp.DiaSemana, disp.HoraInicio.ToString("HH:mm"), disp.HoraFim.ToString("HH:mm"), disp.Ativo, disp.CriadoEm);
    }
}
