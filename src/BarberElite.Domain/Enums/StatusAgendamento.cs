namespace BarberElite.Domain.Enums;

/// <summary>
/// Lifecycle status of an appointment.
/// </summary>
public enum StatusAgendamento
{
    Pendente = 0,
    Confirmado = 1,
    Concluido = 2,
    Cancelado = 3,
    NoShow = 4
}
