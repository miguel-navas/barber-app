using BarberElite.Domain.Common;

namespace BarberElite.Domain.Entities;

/// <summary>
/// Client of the barbershop. Linked to Usuario for login.
/// </summary>
public class Cliente : TenantScopedEntity
{
    public Guid UsuarioId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public DateTime? DataNascimento { get; private set; }
    public DateTime CriadoEm { get; private set; }

    // Construtor para EF Core
    private Cliente() { }

    // Construtor real (DDD correto)
    private Cliente(
        Guid tenantId,
        Guid usuarioId,
        string nome,
        string? telefone,
        DateTime? dataNascimento)
        : base(tenantId)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("UsuarioId é obrigatório.", nameof(usuarioId));

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório.", nameof(nome));

        UsuarioId = usuarioId;
        Nome = nome.Trim();
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
        DataNascimento = dataNascimento;
        CriadoEm = DateTime.UtcNow;
    }

    public static Cliente Criar(
        Guid tenantId,
        Guid usuarioId,
        string nome,
        string? telefone = null,
        DateTime? dataNascimento = null)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId é obrigatório.", nameof(tenantId));

        return new Cliente(
            tenantId,
            usuarioId,
            nome,
            telefone,
            dataNascimento);
    }

    public void Atualizar(string nome, string? telefone, DateTime? dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
        DataNascimento = dataNascimento;
    }
}
