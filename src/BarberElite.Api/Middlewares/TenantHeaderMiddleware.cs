namespace BarberElite.Api.Middlewares;

public sealed class TenantHeaderMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request.Path.Value?.ToLower();

        // ✅ Ignora Swagger e arquivos estáticos
        if (path != null && (path.StartsWith("/swagger") || path.StartsWith("/favicon")))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var value) ||
            !Guid.TryParse(value.ToString(), out var tenantId) ||
            tenantId == Guid.Empty)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Missing or invalid X-Tenant-Id header.");
            return;
        }

        await next(context);
    }
}
