using BarberElite.Domain.Enums;

namespace BarberElite.Application.Disponibilidades.DTOs;

public sealed record DisponibilidadeCreateDto(
    Guid BarbeiroId,
    DiaSemana DiaSemana,
    string HoraInicio, // "09:00"
    string HoraFim     // "18:00"
);

public sealed record DisponibilidadeUpdateDto(
    DiaSemana DiaSemana,
    string HoraInicio,
    string HoraFim,
    bool Ativo
);

public sealed record DisponibilidadeResponseDto(
    Guid Id,
    Guid BarbeiroId,
    DiaSemana DiaSemana,
    string HoraInicio,
    string HoraFim,
    bool Ativo,
    DateTime CriadoEm
);
