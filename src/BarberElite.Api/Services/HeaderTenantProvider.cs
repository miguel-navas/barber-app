using BarberElite.Application.Common.Interfaces;

namespace BarberElite.Api.Services;

public sealed class HeaderTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _http;

    public HeaderTenantProvider(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Guid TenantId
    {
        get
        {
            var ctx = _http.HttpContext;
            if (ctx is null) return Guid.Empty;

            if (!ctx.Request.Headers.TryGetValue("X-Tenant-Id", out var value))
                return Guid.Empty;

            return Guid.TryParse(value.ToString(), out var tenantId) ? tenantId : Guid.Empty;
        }
    }
}
