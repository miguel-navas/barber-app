using BarberElite.Domain.Common;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Tenant (barbearia). Root of multi-tenant boundary. Does not have TenantId.
/// </summary>
public class Tenant : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    private Tenant() { }

    public static Tenant Criar(string nome, string slug)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do tenant é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug do tenant é obrigatório.", nameof(slug));

        return new Tenant
        {
            Nome = nome.Trim(),
            Slug = slug.Trim().ToLowerInvariant()
        };
    }

    public void Atualizar(string nome, bool ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do tenant é obrigatório.", nameof(nome));
        Nome = nome.Trim();
        Ativo = ativo;
    }

    public void Ativar() => Ativo = true;
    public void Inativar() => Ativo = false;
}
