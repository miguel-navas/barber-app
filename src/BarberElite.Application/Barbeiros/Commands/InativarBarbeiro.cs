using BarberElite.Application.Barbeiros.Interfaces;
using MediatR;

namespace BarberElite.Application.Barbeiros.Commands;

public sealed record InativarBarbeiroCommand(Guid Id) : IRequest;

public sealed class InativarBarbeiroHandler : IRequestHandler<InativarBarbeiroCommand>
{
    private readonly IBarbeiroRepository _repo;

    public InativarBarbeiroHandler(IBarbeiroRepository repo) => _repo = repo;

    public async Task Handle(InativarBarbeiroCommand request, CancellationToken ct)
    {
        var b = await _repo.GetByIdAsync(request.Id, ct);
        if (b is null)
            throw new KeyNotFoundException("Barbeiro não encontrado.");

        b.Inativar();

        await _repo.SaveChangesAsync(ct);
    }
}
