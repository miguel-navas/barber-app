namespace BarberElite.Application.Barbeiros.DTOs;

public sealed record BarbeiroCreateDto(Guid UsuarioId, string Nome);

public sealed record BarbeiroUpdateDto(string Nome, bool Ativo);

public sealed record BarbeiroResponseDto(
    Guid Id,
    Guid UsuarioId,
    string Nome,
    bool Ativo,
    DateTime CriadoEm
);
