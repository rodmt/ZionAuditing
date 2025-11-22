using Microsoft.Extensions.DependencyInjection;
using ZionAuditing.Domain.Repositories;
using ZionAuditing.Infrastructure.Persistence;
using ZionAuditing.Infrastructure.Persistence.Repositories;

namespace ZionAuditing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        return services;
    }
}
