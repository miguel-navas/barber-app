using BarberElite.Domain.Enums;

namespace BarberElite.Application.Servicos.DTOs;

public sealed record ServicoCreateDto(
    string Nome,
    string? Descricao,
    int DuracaoMinutos,
    decimal Preco,
    TipoServico Tipo
);

public sealed record ServicoUpdateDto(
    string Nome,
    string? Descricao,
    int DuracaoMinutos,
    decimal Preco,
    TipoServico Tipo,
    bool Ativo
);

public sealed record ServicoResponseDto(
    Guid Id,
    string Nome,
    string? Descricao,
    int DuracaoMinutos,
    decimal Preco,
    TipoServico Tipo,
    bool Ativo,
    DateTime CriadoEm
);
