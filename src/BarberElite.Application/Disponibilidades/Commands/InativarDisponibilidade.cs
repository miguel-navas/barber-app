using BarberElite.Application.Disponibilidades.Interfaces;
using MediatR;

namespace BarberElite.Application.Disponibilidades.Commands;

public sealed record InativarDisponibilidadeCommand(Guid Id) : IRequest;

public sealed class InativarDisponibilidadeHandler : IRequestHandler<InativarDisponibilidadeCommand>
{
    private readonly IDisponibilidadeRepository _repo;

    public InativarDisponibilidadeHandler(IDisponibilidadeRepository repo) => _repo = repo;

    public async Task Handle(InativarDisponibilidadeCommand request, CancellationToken ct)
    {
        var disp = await _repo.GetByIdAsync(request.Id, ct);
        if (disp is null)
            throw new KeyNotFoundException("Disponibilidade não encontrada.");

        disp.Inativar();
        await _repo.SaveChangesAsync(ct);
    }
}
