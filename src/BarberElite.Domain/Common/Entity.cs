namespace BarberElite.Domain.Common;

/// <summary>
/// Base type for all domain entities. Provides identity.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}