using BarberElite.Application.Servicos.DTOs;
using BarberElite.Application.Servicos.Interfaces;
using MediatR;

namespace BarberElite.Application.Servicos.Commands;

public sealed record UpdateServicoCommand(Guid Id, ServicoUpdateDto Dto) : IRequest<ServicoResponseDto>;

public sealed class UpdateServicoHandler : IRequestHandler<UpdateServicoCommand, ServicoResponseDto>
{
    private readonly IServicoRepository _repo;

    public UpdateServicoHandler(IServicoRepository repo) => _repo = repo;

    public async Task<ServicoResponseDto> Handle(UpdateServicoCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct);
        if (s is null)
            throw new KeyNotFoundException("Serviço não encontrado.");

        s.Atualizar(
            request.Dto.Nome,
            request.Dto.DuracaoMinutos,
            request.Dto.Preco,
            request.Dto.Tipo,
            request.Dto.Descricao
        );

        if (request.Dto.Ativo) s.Ativar();
        else s.Desativar();

        await _repo.SaveChangesAsync(ct);

        return new ServicoResponseDto(s.Id, s.Nome, s.Descricao, s.DuracaoMinutos, s.Preco, s.Tipo, s.Ativo, s.CriadoEm);
    }
}
