using BarberElite.Domain.Common;
using BarberElite.Domain.Enums;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Serviço oferecido pela barbearia.
/// Deve possuir duração definida.
/// Preço zero permitido apenas para serviços do tipo Cortesia.
/// </summary>
public class Servico : TenantScopedEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public int DuracaoMinutos { get; private set; }
    public decimal Preco { get; private set; }
    public TipoServico Tipo { get; private set; }
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; }

    private Servico() { }

    private Servico(
        Guid tenantId,
        string nome,
        int duracaoMinutos,
        decimal preco,
        TipoServico tipo,
        string? descricao):base(tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do serviço é obrigatório.", nameof(nome));

        if (duracaoMinutos <= 0)
            throw new ArgumentException("Serviço deve ter duração maior que zero.", nameof(duracaoMinutos));

        if (preco < 0)
            throw new ArgumentException("Preço não pode ser negativo.", nameof(preco));

        if (preco == 0 && tipo != TipoServico.Cortesia)
            throw new ArgumentException("Preço zero permitido apenas para serviços de cortesia.", nameof(preco));

        Nome = nome.Trim();
        Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
        DuracaoMinutos = duracaoMinutos;
        Preco = preco;
        Tipo = tipo;
        CriadoEm = DateTime.UtcNow;
    }

    public static Servico Criar(
        Guid tenantId,
        string nome,
        int duracaoMinutos,
        decimal preco,
        TipoServico tipo,
        string? descricao = null)
    {
        return new Servico(tenantId, nome, duracaoMinutos, preco, tipo, descricao);
    }

    public void Atualizar(
        string nome,
        int duracaoMinutos,
        decimal preco,
        TipoServico tipo,
        string? descricao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do serviço é obrigatório.", nameof(nome));

        if (duracaoMinutos <= 0)
            throw new ArgumentException("Serviço deve ter duração maior que zero.", nameof(duracaoMinutos));

        if (preco < 0)
            throw new ArgumentException("Preço não pode ser negativo.", nameof(preco));

        if (preco == 0 && tipo != TipoServico.Cortesia)
            throw new ArgumentException("Preço zero permitido apenas para serviços de cortesia.", nameof(preco));

        Nome = nome.Trim();
        Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
        DuracaoMinutos = duracaoMinutos;
        Preco = preco;
        Tipo = tipo;
    }

    public void Desativar() => Ativo = false;

    public void Ativar() => Ativo = true;
}
