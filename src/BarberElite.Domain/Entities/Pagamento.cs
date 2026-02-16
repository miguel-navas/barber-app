using BarberElite.Domain.Common;
using BarberElite.Domain.Enums;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Payment for an appointment. Commission is calculated after payment confirmation (business rule).
/// </summary>
public class Pagamento : TenantScopedEntity
{
    public Guid AgendamentoId { get; private set; }
    public decimal Valor { get; private set; }
    public StatusPagamento Status { get; private set; } = StatusPagamento.Pendente;
    public TipoPagamento? MetodoPagamento { get; private set; }
    public string? ExternalId { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    private Pagamento() { }

    public static Pagamento Criar(Guid tenantId, Guid agendamentoId, decimal valor, TipoPagamento? metodoPagamento = null, string? externalId = null)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));
        if (agendamentoId == Guid.Empty)
            throw new ArgumentException("AgendamentoId é obrigatório.", nameof(agendamentoId));
        if (valor <= 0)
            throw new ArgumentException("Valor do pagamento deve ser maior que zero.", nameof(valor));

        return new Pagamento
        {
            AgendamentoId = agendamentoId,
            Valor = valor,
            MetodoPagamento = metodoPagamento,
            ExternalId = string.IsNullOrWhiteSpace(externalId) ? null : externalId.Trim()
        };
    }

    /// <summary>
    /// Mark as paid. After this, commission can be calculated (business rule).
    /// </summary>
    public void MarcarComoPago(TipoPagamento metodoPagamento, DateTime? dataPagamento = null)
{
    if (Status == StatusPagamento.Pago)
        throw new InvalidOperationException("Pagamento já está pago.");
    if (Status == StatusPagamento.Cancelado)
        throw new InvalidOperationException("Pagamento cancelado não pode ser marcado como pago.");

    MetodoPagamento = metodoPagamento;

    var pagoEm = dataPagamento ?? DateTime.UtcNow;
    DataPagamento = pagoEm.Kind == DateTimeKind.Utc ? pagoEm : pagoEm.ToUniversalTime();

    Status = StatusPagamento.Pago;
}


    public void Cancelar()
    {
        if (Status == StatusPagamento.Pago)
            throw new InvalidOperationException("Pagamento já realizado não pode ser cancelado.");
        Status = StatusPagamento.Cancelado;
    }

    public bool PermiteCalcularComissao => Status == StatusPagamento.Pago;
}
