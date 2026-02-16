using BarberElite.Domain.Common;
using BarberElite.Domain.Enums;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Appointment. Business rules: no double booking for same barber; cancellation allowed until 2h before.
/// </summary>
public class Agendamento : TenantScopedEntity
{
    private static readonly TimeSpan MinHorasParaCancelamento = TimeSpan.FromHours(2);

    public Guid ClienteId { get; private set; }
    public Guid BarbeiroId { get; private set; }
    public Guid ServicoId { get; private set; }
    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }
    public StatusAgendamento Status { get; private set; } = StatusAgendamento.Pendente;
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    private Agendamento() { }

    public static Agendamento Criar(Guid tenantId, Guid clienteId, Guid barbeiroId, Guid servicoId, DateTime dataHoraInicioUtc, int duracaoMinutos)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));
        if (clienteId == Guid.Empty)
            throw new ArgumentException("ClienteId é obrigatório.", nameof(clienteId));
        if (barbeiroId == Guid.Empty)
            throw new ArgumentException("BarbeiroId é obrigatório.", nameof(barbeiroId));
        if (servicoId == Guid.Empty)
            throw new ArgumentException("ServicoId é obrigatório.", nameof(servicoId));
        if (duracaoMinutos <= 0)
            throw new ArgumentException("Duração deve ser maior que zero.", nameof(duracaoMinutos));

       if (dataHoraInicioUtc.Kind != DateTimeKind.Utc)
        throw new ArgumentException("DataHoraInicio deve estar em UTC.", nameof(dataHoraInicioUtc));

        if (dataHoraInicioUtc < DateTime.UtcNow)
            throw new InvalidOperationException("Agendamento não pode ser no passado.");

        var fim = dataHoraInicioUtc.AddMinutes(duracaoMinutos);

        return new Agendamento
        {
            ClienteId = clienteId,
            BarbeiroId = barbeiroId,
            ServicoId = servicoId,
            DataHoraInicio = dataHoraInicioUtc,
            DataHoraFim = fim
        };
    }

    /// <summary>
    /// Cancellation allowed until 2 hours before start (business rule).
    /// </summary>
    public void Cancelar(DateTime? dataHoraReferencia = null)
    {
        if (Status == StatusAgendamento.Concluido)
            throw new InvalidOperationException("Agendamento concluído não pode ser cancelado.");

        if (Status == StatusAgendamento.Cancelado)
            throw new InvalidOperationException("Agendamento já está cancelado.");

        var referencia = (dataHoraReferencia ?? DateTime.UtcNow);
        if (referencia.Kind != DateTimeKind.Utc)
            referencia = referencia.ToUniversalTime();

        var limiteCancelamento = DataHoraInicio.Subtract(MinHorasParaCancelamento);

        if (referencia > limiteCancelamento)
            throw new InvalidOperationException("Cancelamento permitido apenas até 2 horas antes do horário do agendamento.");

        Status = StatusAgendamento.Cancelado;
    }

    public void Confirmar()
    {
        if (Status != StatusAgendamento.Pendente)
            throw new InvalidOperationException("Apenas agendamentos pendentes podem ser confirmados.");
        Status = StatusAgendamento.Confirmado;
    }

    public void Concluir()
    {
        if (Status != StatusAgendamento.Confirmado)
            throw new InvalidOperationException("Apenas agendamentos confirmados podem ser concluídos.");
        Status = StatusAgendamento.Concluido;
    }

    /// <summary>
    /// Checks if this appointment overlaps with another (same barber). Used to enforce no double booking.
    /// </summary>
    public bool Sobrepoe(DateTime inicioOutro, DateTime fimOutro)
    {
        return BarbeiroId != Guid.Empty
            && DataHoraInicio < fimOutro
            && DataHoraFim > inicioOutro;
    }
}
