using BarberElite.Application.Disponibilidades.DTOs;
using BarberElite.Application.Disponibilidades.Interfaces;
using MediatR;

namespace BarberElite.Application.Disponibilidades.Commands;

public sealed record UpdateDisponibilidadeCommand(Guid Id, DisponibilidadeUpdateDto Dto) : IRequest<DisponibilidadeResponseDto>;

public sealed class UpdateDisponibilidadeHandler : IRequestHandler<UpdateDisponibilidadeCommand, DisponibilidadeResponseDto>
{
    private readonly IDisponibilidadeRepository _repo;

    public UpdateDisponibilidadeHandler(IDisponibilidadeRepository repo) => _repo = repo;

    public async Task<DisponibilidadeResponseDto> Handle(UpdateDisponibilidadeCommand request, CancellationToken ct)
    {
        var disp = await _repo.GetByIdAsync(request.Id, ct);
        if (disp is null)
            throw new KeyNotFoundException("Disponibilidade não encontrada.");

        if (!TimeOnly.TryParse(request.Dto.HoraInicio, out var inicio))
            throw new ArgumentException("HoraInicio inválida. Use formato HH:mm.");
        if (!TimeOnly.TryParse(request.Dto.HoraFim, out var fim))
            throw new ArgumentException("HoraFim inválida. Use formato HH:mm.");

        // Se estiver ativando ou mudando horário/dia, validar sobreposição
        if (request.Dto.Ativo)
        {
            var existentes = await _repo.ListAtivasByBarbeiroDiaAsync(disp.BarbeiroId, request.Dto.DiaSemana, ct);
            foreach (var e in existentes.Where(x => x.Id != disp.Id))
            {
                var sobrepoe = inicio < e.HoraFim && fim > e.HoraInicio;
                if (sobrepoe)
                    throw new InvalidOperationException("Conflito: horário sobrepõe outra disponibilidade ativa do barbeiro.");
            }
        }

        // Atualiza os campos
        // OBS: seu Domain Atualizar não recebe DiaSemana; se você quiser permitir troca de dia, podemos ajustar Domain.
        // Por enquanto: se DiaSemana mudou, sugerido criar outra e inativar esta. (mantém invariantes.)
        if (request.Dto.DiaSemana != disp.DiaSemana)
            throw new InvalidOperationException("Para alterar o DiaSemana, crie uma nova disponibilidade e inative a antiga.");

        disp.Atualizar(inicio, fim, request.Dto.Ativo);

        await _repo.SaveChangesAsync(ct);

        return new DisponibilidadeResponseDto(disp.Id, disp.BarbeiroId, disp.DiaSemana, disp.HoraInicio.ToString("HH:mm"), disp.HoraFim.ToString("HH:mm"), disp.Ativo, disp.CriadoEm);
    }
}
