using BarberElite.Domain.Common;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Barber. Linked to Usuario for login. Has availability and commissions.
/// </summary>
public class Barbeiro : TenantScopedEntity
{
    public Guid UsuarioId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; }

    // Construtor para EF Core
    private Barbeiro() { }

    // Construtor real (DDD correto)
    private Barbeiro(Guid tenantId, Guid usuarioId, string nome)
        : base(tenantId)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("UsuarioId é obrigatório.", nameof(usuarioId));

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do barbeiro é obrigatório.", nameof(nome));

        UsuarioId = usuarioId;
        Nome = nome.Trim();
        CriadoEm = DateTime.UtcNow;
    }

    public static Barbeiro Criar(Guid tenantId, Guid usuarioId, string nome)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        return new Barbeiro(tenantId, usuarioId, nome);
    }

    public void Atualizar(string nome, bool ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do barbeiro é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        Ativo = ativo;
    }

    public void Ativar() => Ativo = true;
    public void Inativar() => Ativo = false;
}
