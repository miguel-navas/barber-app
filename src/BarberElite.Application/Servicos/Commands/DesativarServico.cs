using BarberElite.Application.Servicos.Interfaces;
using MediatR;

namespace BarberElite.Application.Servicos.Commands;

public sealed record DesativarServicoCommand(Guid Id) : IRequest;

public sealed class DesativarServicoHandler : IRequestHandler<DesativarServicoCommand>
{
    private readonly IServicoRepository _repo;
    public DesativarServicoHandler(IServicoRepository repo) => _repo = repo;

    public async Task Handle(DesativarServicoCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct);
        if (s is null)
            throw new KeyNotFoundException("Serviço não encontrado.");

        s.Desativar();
        await _repo.SaveChangesAsync(ct);
    }
}
