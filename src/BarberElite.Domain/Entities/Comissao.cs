using BarberElite.Domain.Common;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Commission for a barber from a payment. Calculated after payment confirmation (business rule).
/// </summary>
public class Comissao : TenantScopedEntity
{
    public Guid BarbeiroId { get; private set; }
    public Guid PagamentoId { get; private set; }
    public decimal Valor { get; private set; }
    public decimal PercentualAplicado { get; private set; }
    public bool Pago { get; private set; }
    public DateTime? DataPagamentoComissao { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Construtor para EF Core
    private Comissao() { }

    // Construtor real (DDD correto)
    private Comissao(Guid tenantId, Guid barbeiroId, Guid pagamentoId, decimal valorPagamento, decimal percentualComissao)
        : base(tenantId)
    {
        if (barbeiroId == Guid.Empty)
            throw new ArgumentException("BarbeiroId é obrigatório.", nameof(barbeiroId));

        if (pagamentoId == Guid.Empty)
            throw new ArgumentException("PagamentoId é obrigatório.", nameof(pagamentoId));

        if (valorPagamento <= 0)
            throw new ArgumentException("Valor do pagamento deve ser maior que zero.", nameof(valorPagamento));

        if (percentualComissao < 0 || percentualComissao > 100)
            throw new ArgumentException("Percentual de comissão deve estar entre 0 e 100.", nameof(percentualComissao));

        var valor = Math.Round(valorPagamento * (percentualComissao / 100m), 2, MidpointRounding.AwayFromZero);

        BarbeiroId = barbeiroId;
        PagamentoId = pagamentoId;
        Valor = valor;
        PercentualAplicado = percentualComissao;
        CriadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Create commission after payment is confirmed (business rule: comissão calculada após confirmação de pagamento).
    /// </summary>
    public static Comissao Criar(Guid tenantId, Guid barbeiroId, Guid pagamentoId, decimal valorPagamento, decimal percentualComissao)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        return new Comissao(tenantId, barbeiroId, pagamentoId, valorPagamento, percentualComissao);
    }

    public void MarcarComoPago(DateTime? dataPagamento = null)
    {
        if (Pago)
            throw new InvalidOperationException("Comissão já foi paga.");

        Pago = true;

        var pagoEm = dataPagamento ?? DateTime.UtcNow;
        DataPagamentoComissao = pagoEm.Kind == DateTimeKind.Utc ? pagoEm : pagoEm.ToUniversalTime();
    }
}
