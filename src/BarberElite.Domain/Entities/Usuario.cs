using BarberElite.Domain.Common;
using BarberElite.Domain.Enums;

namespace BarberElite.Domain.Entities;

/// <summary>
/// User account (login). Identity for Cliente, Barbeiro or Administrador.
/// </summary>
public class Usuario : TenantScopedEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Nome { get; private set; } = string.Empty;
    public PapelUsuario Papel { get; private set; }
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; }

    // Construtor para EF Core
    private Usuario() { }

    // Construtor real (DDD correto)
    private Usuario(Guid tenantId, string email, string passwordHash, string nome, PapelUsuario papel)
        : base(tenantId)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash é obrigatório.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Nome = nome.Trim();
        Papel = papel;
        CriadoEm = DateTime.UtcNow;
    }

    public static Usuario Criar(Guid tenantId, string email, string passwordHash, string nome, PapelUsuario papel)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        return new Usuario(tenantId, email, passwordHash, nome, papel);
    }

    public void Atualizar(string nome, bool ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        Ativo = ativo;
    }

    public void AlterarSenha(string novoPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(novoPasswordHash))
            throw new ArgumentException("Novo hash de senha é obrigatório.", nameof(novoPasswordHash));

        PasswordHash = novoPasswordHash;
    }

    public void Ativar() => Ativo = true;
    public void Inativar() => Ativo = false;
}
