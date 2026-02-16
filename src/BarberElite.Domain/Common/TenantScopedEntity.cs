using System;

namespace BarberElite.Domain.Common;

/// <summary>
/// Base entity for all tenant-scoped aggregates and entities.
/// TenantId should be immutable after creation.
/// </summary>
public abstract class TenantScopedEntity : Entity
{
    public Guid TenantId { get; private set; }

    // For EF Core / serializers
    protected TenantScopedEntity() { }

    protected TenantScopedEntity(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        TenantId = tenantId;
    }
}
