using BarberElite.Domain.Common;
using BarberElite.Domain.Enums;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Weekly time slot when a barber is available (recurring).
/// </summary>
public class DisponibilidadeHorario : TenantScopedEntity
{
    public Guid BarbeiroId { get; private set; }
    public DiaSemana DiaSemana { get; private set; }
    public TimeOnly HoraInicio { get; private set; }
    public TimeOnly HoraFim { get; private set; }
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; }

    // Construtor para EF Core
    private DisponibilidadeHorario() { }

    // Construtor real (DDD correto)
    private DisponibilidadeHorario(Guid tenantId, Guid barbeiroId, DiaSemana diaSemana, TimeOnly horaInicio, TimeOnly horaFim)
        : base(tenantId)
    {
        if (barbeiroId == Guid.Empty)
            throw new ArgumentException("BarbeiroId é obrigatório.", nameof(barbeiroId));

        if (horaFim <= horaInicio)
            throw new ArgumentException("Hora fim deve ser posterior à hora início.", nameof(horaFim));

        BarbeiroId = barbeiroId;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
        CriadoEm = DateTime.UtcNow;
    }

    public static DisponibilidadeHorario Criar(Guid tenantId, Guid barbeiroId, DiaSemana diaSemana, TimeOnly horaInicio, TimeOnly horaFim)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        return new DisponibilidadeHorario(tenantId, barbeiroId, diaSemana, horaInicio, horaFim);
    }

    public void Atualizar(TimeOnly horaInicio, TimeOnly horaFim, bool ativo)
    {
        if (horaFim <= horaInicio)
            throw new ArgumentException("Hora fim deve ser posterior à hora início.", nameof(horaFim));

        HoraInicio = horaInicio;
        HoraFim = horaFim;
        Ativo = ativo;
    }

    public void Ativar() => Ativo = true;
    public void Inativar() => Ativo = false;

    /// <summary>
    /// Returns true if the given time falls within this slot (same day, between start and end).
    /// </summary>
    public bool ContemHorario(DiaSemana dia, TimeOnly hora)
    {
        return Ativo && DiaSemana == dia && hora >= HoraInicio && hora < HoraFim;
    }
}
