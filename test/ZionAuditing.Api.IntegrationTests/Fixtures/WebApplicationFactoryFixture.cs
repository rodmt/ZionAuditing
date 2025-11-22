using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ZionAuditing.Domain.Repositories;
using NSubstitute;

namespace ZionAuditing.Api.IntegrationTests.Fixtures;

public sealed class WebApplicationFactoryFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Replace repository with mock for testing
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAuditLogRepository));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped(_ => Substitute.For<IAuditLogRepository>());
        });

        builder.UseEnvironment("Testing");
    }
}
